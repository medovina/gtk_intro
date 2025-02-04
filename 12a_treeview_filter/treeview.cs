using Gdk;
using Gtk;

class Movie {
    public int year;
    public string title;
    public string director;

    public Movie(int year, string title, string director) {
        this.year = year; this.title = title; this.director = director;
    }
}

class MyWindow : Gtk.Window {
    List<Movie> movies = new List<Movie>();

    ListStore store = new ListStore(typeof(int), typeof(string), typeof(string));
    TreeModelFilter filter;
    TreeView tree_view;
    SearchEntry entry;

    public MyWindow() : base("movies") {
        read_movies();
        fill();

        filter = new TreeModelFilter(store, null);
        tree_view = new TreeView(filter);
        
        string[] fields = { "year", "title", "director" };
        for (int i = 0 ; i < 3 ; ++i) {
            TreeViewColumn c = new TreeViewColumn(fields[i], new CellRendererText(), "text", i);
            c.SortColumnId = i;     // make column sortable
            tree_view.AppendColumn(c);
        }
        
        ScrolledWindow scrolled = new ScrolledWindow();
        scrolled.Add(tree_view);

        entry = new SearchEntry();
        entry.SearchChanged += on_search_changed;
        entry.Activated += on_activate;
        filter.VisibleFunc = is_row_visible;

        Box vbox = new Box(Orientation.Vertical, 0);
        vbox.PackStart(scrolled, true, true, 0);
        vbox.Add(entry);
        Add(vbox);

        tree_view.CursorChanged += on_cursor_changed;

        Resize(600, 400);
    }

    bool is_row_visible(ITreeModel model, TreeIter iter) {
        string s = entry.Text;
        string title = (string) store.GetValue(iter, 1);
        return title.Contains(s, StringComparison.OrdinalIgnoreCase);
    }

    private void on_search_changed(object? sender, EventArgs e) {
        filter.Refilter();
    }

    private void on_activate(object? sender, EventArgs e) {
        Console.WriteLine("activate");
    }

    void read_movies() {
        using (StreamReader sr = new StreamReader("movies.csv"))
            while (sr.ReadLine() is string line) {
                string[] fields = line.Split(',').Select(s => s.Trim()).ToArray();
                movies.Add(new Movie(int.Parse(fields[0]), fields[1], fields[2]));
            }
    }

    void fill() {
        store.Clear();
        foreach (Movie m in movies) {
            TreeIter i = store.Append();
            store.SetValues(i, m.year, m.title, m.director);
        }
    }

    int selected() {
        TreePath[] rows = tree_view.Selection.GetSelectedRows();
        return rows.Length > 0 ? rows[0].Indices[0] : -1;
    }

    void on_cursor_changed(object? sender, EventArgs args) {
        int i = selected();
        if (i >= 0) {
            Console.WriteLine($"selected movie: {movies[i].title}");
        }
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
