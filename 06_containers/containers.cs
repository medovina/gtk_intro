using Gdk;
using Gtk;
using static Gtk.Orientation;

class MyWindow : Gtk.Window {
    public MyWindow() : base("containers") {
        Box top_hbox = new Box(Horizontal, 5);
        top_hbox.Add(new Label("one"));
        top_hbox.Add(new Button("button 1"));
        top_hbox.Add(new Label("two"));
        top_hbox.Add(new Button("button 2"));

        Box left_vbox = new Box(Vertical, 5);
        left_vbox.Add(new Label("three"));
        left_vbox.Add(new Button("button 3"));
        left_vbox.Add(new Label("four"));
        left_vbox.PackStart(new Button("button 4"), true, true, 0);

        Grid grid = new Grid();
        grid.ColumnSpacing = 5;
        grid.RowSpacing = 3;
        grid.Attach(new Label("field 1:"), 0, 0, 1, 1);
        grid.Attach(new Entry(), 1, 0, 1, 1);
        grid.Attach(new Label("field 2:"), 0, 1, 1, 1);
        grid.Attach(new Entry(), 1, 1, 1, 1);
        grid.Attach(new Image("cube.png"), 0, 2, 2, 1);

        Box hbox2 = new Box(Horizontal, 5);
        hbox2.Add(left_vbox);
        hbox2.Add(new Separator(Vertical));
        hbox2.Add(grid);

        Box vbox = new Box(Vertical, 5);
        vbox.Add(top_hbox);
        vbox.Add(new Separator(Horizontal));
        vbox.Add(hbox2);
        vbox.Margin = 8;
        Add(vbox);
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
