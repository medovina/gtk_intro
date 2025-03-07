﻿using Gdk;
using Gtk;
using Key = Gdk.Key;

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
    TreeView tree_view;

    public MyWindow() : base("movies") {
        read_movies();
        fill();

        tree_view = new TreeView(store);
        string[] fields = { "year", "title", "director" };
        for (int i = 0 ; i < 3 ; ++i) {
            TreeViewColumn c = new TreeViewColumn(fields[i], new CellRendererText(), "text", i);
            c.SortColumnId = i;     // make column sortable
            tree_view.AppendColumn(c);
        }
        
        ScrolledWindow scrolled = new ScrolledWindow();
        scrolled.Add(tree_view);
        Add(scrolled);

        tree_view.CursorChanged += on_cursor_changed;

        Resize(600, 400);
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
        // Retrieve the selected movie title from the ListStore.
        if (tree_view.Selection.GetSelected(out TreeIter iter)) {
            string title = (string) store.GetValue(iter, 1);
            Console.WriteLine($"selected movie: {title}");
        }

        // Alternative: retrieve the selected title from the List of Movie objects.
        int i = selected();
        if (i >= 0) {
            Console.WriteLine($"selected movie: {movies[i].title}");
        }
    }

    protected override bool OnKeyPressEvent(EventKey e) {
        if (e.Key == Key.Delete) {
            int i = selected();
            if (i >= 0) {
                movies.RemoveAt(i);
                fill();
            }
        }
        return true;
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
