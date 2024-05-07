using Cairo;
using Gdk;
using Gtk;
using Color = Cairo.Color;

class Area : DrawingArea {
    Color black = new Color(0, 0, 0),
          blue = new Color(0, 0, 1),
          light_green = new Color(0.56, 0.93, 0.56),
          red = new Color(1, 0, 0);

    protected override bool OnDrawn (Context c) {
        using (LinearGradient g = new LinearGradient(x0: 150, y0: 100, x1: 250, y1: 100)) {
            g.AddColorStop(0.0, red);
            g.AddColorStop(1.0, light_green);

            c.SetSource(g);
            c.Rectangle(x: 100, y: 50, width: 200, height: 100);
            c.Fill();
        }

        using (RadialGradient r = new RadialGradient(cx0: 200, cy0: 300, radius0: 25,
                                                     cx1: 200, cy1: 300, radius1: 75)) {
            r.AddColorStop(0.0, black);
            r.AddColorStop(1.0, light_green);

            c.SetSource(r);
            c.Arc(xc: 200, yc: 300, radius: 75, angle1: 0, angle2: 2 * Math.PI);
            c.Fill();
        }

        return true;
    }
}

class MyWindow : Gtk.Window {
    public MyWindow() : base("gradients") {
        Resize(400, 400);
        Add(new Area());  // add an Area to the window
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
