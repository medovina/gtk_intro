using Gdk;
using Gtk;

class MyWindow : Gtk.Window {
    TextView text_view = new TextView();
    MenuItem uppercase_item, lowercase_item;

    public MyWindow() : base("edit") {
        Menu fileMenu = new Menu();
        fileMenu.Append(item("Open...", on_open));
        fileMenu.Append(item("Exit", on_exit));

        Menu transformMenu = new Menu();
        uppercase_item = item("Uppercase", on_uppercase);
        transformMenu.Append(uppercase_item);
        lowercase_item = item("Lowercase", on_lowercase);
        transformMenu.Append(lowercase_item);
        uppercase_item.Sensitive = lowercase_item.Sensitive = false;

        Menu documentMenu = new Menu();
        documentMenu.Append(item("Information", on_information));

        MenuBar bar = new MenuBar();
        bar.Append(submenu("File", fileMenu));
        bar.Append(submenu("Transform", transformMenu, on_open_transform_menu));
        bar.Append(submenu("Document", documentMenu));

        Box vbox = new Box(Orientation.Vertical, 0);
        vbox.Add(bar);
        ScrolledWindow scrolled = new ScrolledWindow();
        scrolled.Add(text_view);
        vbox.PackStart(scrolled, true, true, 0);
        Add(vbox);

        text_view.Monospace = true;

        Resize(600, 400);
    }

    static MenuItem item(string name, EventHandler handler) {
        MenuItem i = new MenuItem(name);
        i.Activated += handler;
        return i;
    }

    static MenuItem submenu(string name, Menu menu, EventHandler? handler = null) {
        MenuItem i = new MenuItem(name);
        i.Submenu = menu;
        if (handler != null)
            i.Activated += handler;
        return i;
    }

    void on_open(object? sender, EventArgs args) {
        using (FileChooserDialog d = new FileChooserDialog(
                "Open...", this, FileChooserAction.Open,
                "Cancel", ResponseType.Cancel, "Open", ResponseType.Ok))
            if (d.Run() == (int) ResponseType.Ok)
                using (StreamReader sr = new StreamReader(d.Filename))
                    text_view.Buffer.Text = sr.ReadToEnd();
    }

    void on_exit(object? sender, EventArgs args) {
        Application.Quit();
    }

    void on_open_transform_menu(object? sender, EventArgs args) {
        uppercase_item.Sensitive = lowercase_item.Sensitive = text_view.Buffer.HasSelection;
    }

    void replace(Func<string, string> f) {
        TextBuffer buf = text_view.Buffer;

        if (buf.GetSelectionBounds(out TextIter start, out TextIter end)) {
            string text = buf.GetText(start, end, false);
            buf.Delete(ref start, ref end);
            buf.Insert(ref start, f(text));
        }
    }

    void on_uppercase(object? sender, EventArgs args) {
        replace(s => s.ToUpper());
    }

    void on_lowercase(object? sender, EventArgs args) {
        replace(s => s.ToLower());
    }

    void on_information(object? sender, EventArgs args) {
        string text = text_view.Buffer.Text;
        int lines = text.Split('\n').Length;
        int words = text.Split().Where(w => w != "").Count();
        using (MessageDialog d =
            new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                              ButtonsType.Ok, "{0} lines, {1} words, {2} characters",
                              lines, words, text.Length))
            d.Run();
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
