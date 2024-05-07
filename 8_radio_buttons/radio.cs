using Cairo;
using Gdk;
using Gtk;
using Color = Cairo.Color;

class Area : DrawingArea {
    public Color color = new Color(1, 0, 0);

    public Area() {
        SetSizeRequest(0, 100);
    }

    public void setColor(Color c) {
        color = c;
        QueueDraw();
    }

    protected override bool OnDrawn(Context c) {
        c.SetSourceColor(color);
        c.Paint();
        return true;
    }
}

class MyWindow : Gtk.Window {
    Area area = new Area();

    public MyWindow() : base("colors") {
        Box hbox = new Box(Gtk.Orientation.Horizontal, 5);
        RadioButton r = new RadioButton("red");
        RadioButton g = new RadioButton(r, "green");
        RadioButton b = new RadioButton(r, "blue");
        r.Clicked += on_red_clicked;
        g.Clicked += on_green_clicked;
        b.Clicked += on_blue_clicked;
        hbox.Add(r);
        hbox.Add(g);
        hbox.Add(b);
        hbox.Margin = 5;

        Box vbox = new Box(Gtk.Orientation.Vertical, 5);
        vbox.Add(hbox);
        vbox.Add(area);

        Add(vbox);
    }

    void on_red_clicked(object? sender, EventArgs e) {
        area.setColor(new Color(1, 0, 0));
    }

    void on_green_clicked(object? sender, EventArgs e) {
        area.setColor(new Color(0, 1, 0));
    }

    void on_blue_clicked(object? sender, EventArgs e) {
        area.setColor(new Color(0, 0, 1));
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
