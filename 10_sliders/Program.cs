using Gdk;
using Gtk;
using static Gtk.Orientation;

class MyWindow : Gtk.Window {
    string[] nums = { "",
        "one", "two", "three", "four", "five",
        "six", "seven", "eight", "nine", "ten" };

    Scale horiz = new Scale(Horizontal, 1, 10, 1);
    Scale vert = new Scale(Vertical, 1, 10, 1);
    SpinButton spinner = new SpinButton(1, 10, 1);
    Label label = new Label("one");

    public MyWindow() : base("sliders") {
        Box row = new Box(Horizontal, 0);
        row.PackStart(horiz, true, true, 0);
        horiz.ValueChanged += on_horiz_changed;
        row.Add(vert);
        vert.ValueChanged += on_vert_changed;

        Box row2 = new Box(Horizontal, 8);
        row2.Add(spinner);
        spinner.ValueChanged += on_spinner_changed;
        row2.Add(label);

        Box vbox = new Box(Vertical, 0);
        vbox.PackStart(row, expand: true, fill: true, 0);
        vbox.Add(row2);
        Add(vbox);
        vbox.Margin = 8;
    }

    void update(double val) {
        horiz.Value = vert.Value = spinner.Value = val;
        label.Text = nums[(int) val];
    }

    void on_horiz_changed(object? sender, EventArgs args) {
        update(horiz.Value);
    }

    void on_vert_changed(object? sender, EventArgs args) {
        update(vert.Value);
    }

    void on_spinner_changed(object? sender, EventArgs arg) {
        update(spinner.Value);
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
        w.Resize(150, 150);
        w.ShowAll();
        Application.Run();
    }
}
