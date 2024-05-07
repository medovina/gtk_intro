using System.Runtime.InteropServices;
using Cairo;
using Gdk;
using Gtk;
using static Gdk.EventMask;

class Area : DrawingArea {
    ImageSurface image;
    bool dragging = false;
    double start_x, start_y;    // start position of mouse drag
    double end_x, end_y;        // end position of drag

    public Area() {
        image = new ImageSurface(Format.Rgb24, 400, 400);
        using (Context c = new Context(image)) {
            c.SetSourceRGB(1, 1, 1);    // white
            c.Paint();
        }

        AddEvents((int) (ButtonPressMask | ButtonReleaseMask | PointerMotionMask));
    }

    void line(Context c) {
        c.SetSourceRGB(0, 0, 0);
        c.LineWidth = 3;
        c.MoveTo(start_x, start_y);
        c.LineTo(end_x, end_y);
        c.Stroke();
    }

    (int, int)[] dirs = { (-1, 0), (1, 0), (0, -1), (0, 1) };

    void flood(int start_x, int start_y) {
        image.Flush();

        byte[] b = new byte[image.Stride * image.Height];
        Marshal.Copy(image.DataPtr, b, 0, b.Length);

        var stack = new Stack<(int, int)>();

        // choose a random color
        Random rand = new Random();
        byte[] color = new byte[3];
        rand.NextBytes(color);

        void visit(int x, int y) {
            int i = y * image.Stride + 4 * x;
            if (b[i] == 255 && b[i + 1] == 255 && b[i + 2] == 255) {
                for (int j = 0 ; j < 3 ; ++j)
                    b[i + j] = color[j];
                stack.Push((x, y));
            }
        }

        visit(start_x, start_y);

        while (stack.TryPop(out (int x, int y) p)) {
            foreach ((int dx, int dy) in dirs) {
                int x1 = p.x + dx, y1 = p.y + dy;
                if (0 <= x1 && x1 < image.Width && 0 <= y1 && y1 < image.Height)
                    visit(x1, y1);
            }
        }

        Marshal.Copy(b, 0, image.DataPtr, b.Length);

        image.MarkDirty();
    }

    protected override bool OnDrawn (Context c) {
        c.SetSourceSurface(image, 0, 0);
        c.Paint();
        
        if (dragging)
            line(c);
        return true;
    }

    protected override bool OnButtonPressEvent (EventButton e) {
        if (e.Button == 1) {
            dragging = true;
            (start_x, start_y) = (end_x, end_y) = (e.X, e.Y);
        } else if (e.Button == 3)
            flood((int) e.X, (int) e.Y);
        QueueDraw();
        return true;
    }

    protected override bool OnMotionNotifyEvent(EventMotion e) {
        if (dragging) {
            (end_x, end_y) = (e.X, e.Y);
            QueueDraw();
        }
        return true;
    }

    protected override bool OnButtonReleaseEvent (EventButton e) {
        if (e.Button == 1) {
            dragging = false;
            using (Context c = new Context(image))
                line(c);
            QueueDraw();
        }
        return true;
    }
}

class MyWindow : Gtk.Window {
    Area area;

    public MyWindow() : base("draw") {
        Resize(400, 400);
        area = new Area();
        Add(area);
    }

    protected override bool OnDeleteEvent(Event e) {
        Application.Quit();
        return true;
    }
}

class Hello {
    static void Main() {
        Application.Init();
        MyWindow w = new MyWindow();
        w.ShowAll();
        Application.Run();
    }
} 
