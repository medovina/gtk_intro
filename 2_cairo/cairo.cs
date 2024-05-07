using Cairo;
using Gdk;
using Gtk;
using Color = Cairo.Color;

class Area : DrawingArea {
    Color black = new Color(0, 0, 0),
          blue = new Color(0, 0, 1),
          light_green = new Color(0.56, 0.93, 0.56);

    protected override bool OnDrawn (Context c) {
        // draw a triangle
        c.SetSourceColor(black);
        c.LineWidth = 5;
        c.MoveTo(100, 50);
        c.LineTo(150, 150);
        c.LineTo(50, 150);
        c.ClosePath();
        c.StrokePreserve();     // draw outline, preserving path
        c.SetSourceColor(light_green);
        c.Fill();               // fill the inside

        // draw a circle
        c.SetSourceColor(blue);
        c.Arc(xc: 300, yc: 100, radius: 50, angle1: 0.0, angle2: 2 * Math.PI);
        c.Fill();

        // draw a rectangle
        c.SetSourceColor(black);
        c.Rectangle(x: 100, y: 200, width: 200, height: 100);
        c.Stroke();

        // draw text centered in the rectangle
        (int cx, int cy) = (200, 250);  // center of rectangle
        string s = "hello, cairo";
        c.SetFontSize(30);
        TextExtents te = c.TextExtents(s);
        c.MoveTo(cx - (te.Width / 2 + te.XBearing), cy - (te.Height / 2 + te.YBearing));
        c.ShowText(s);

        return true;
    }
}

class MyWindow : Gtk.Window {
    public MyWindow() : base("cairo") {
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
