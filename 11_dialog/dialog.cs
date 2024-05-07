using Gdk;
using Gtk;
using Window = Gtk.Window;
using static Gtk.Orientation;

class City {
    public string name;
    public int population;
    public bool is_capital;

    public City(string name, int population, bool is_capital) {
        this.name = name; this.population = population; this.is_capital = is_capital;
    }
}

class CityDialog : Dialog {
    public Entry name = new Entry();
    public SpinButton population = new SpinButton(0, 5_000_000, 100_000);
    public CheckButton capital = new CheckButton("Capital city");

    public CityDialog(Window parent, City city)
            : base("Edit", parent, DialogFlags.Modal,
                   "OK", ResponseType.Ok, "Cancel", ResponseType.Cancel) {
        name.Text = city.name;
        population.Value = city.population;
        capital.Active = city.is_capital;

        Grid grid = new Grid();

        Label name_label = new Label("Name");
        name_label.Halign = Align.End;
        grid.Attach(name_label, 0, 0, 1, 1);
        grid.Attach(name, 1, 0, 1, 1);

        Label pop_label = new Label("Population");
        pop_label.Halign = Align.End;
        grid.Attach(pop_label, 0, 1, 1, 1);
        grid.Attach(population, 1, 1, 1, 1);

        grid.Attach(capital, 1, 2, 1, 1);

        grid.ColumnSpacing = 10;
        grid.RowSpacing = 5;
        grid.Margin = 5;
        ContentArea.Add(grid);
        ShowAll();
    }
}

class MyWindow : Window {
    City city;
    Label name = new Label(), population = new Label(), is_capital = new Label();

    public MyWindow() : base("city") {
        city = new City("Paris", 2_100_000, true);
        Grid grid = new Grid();
        grid.Attach(new Label("name"), 0, 0, 1, 1);
        grid.Attach(name, 1, 0, 1, 1);
        grid.Attach(new Label("population"), 0, 1, 1, 1);
        grid.Attach(population, 1, 1, 1, 1);
        grid.Attach(new Label("capital"), 0, 2, 1, 1);
        grid.Attach(is_capital, 1, 2, 1, 1);
        grid.ColumnSpacing = 10;
        grid.RowSpacing = 5;
        foreach (Widget w in grid)
            w.Halign = Align.Start;

        Box row = new Box(Horizontal, 5);
        Button edit = new Button("Edit");
        row.Add(edit);
        edit.Clicked += on_edit;
        Button quit = new Button("Quit");
        row.Add(quit);
        quit.Clicked += on_quit;

        Box vbox = new Box(Vertical, 5);
        vbox.Add(grid);
        vbox.Add(row);
        Add(vbox);
        vbox.Margin = 5;

        update_labels();
    }

    void update_labels() {
        name.Text = city.name;
        population.Text = city.population.ToString();
        is_capital.Text = city.is_capital ? "yes" : "no";
    }

    void on_edit(object? sender, EventArgs args) {
        using (CityDialog d = new CityDialog(this, city))
            if (d.Run() == (int) ResponseType.Ok) {
                city.name = d.name.Text;
                city.population = (int) d.population.Value;
                city.is_capital = d.capital.Active;
                update_labels();
            }
    }

    void on_quit(object? sender, EventArgs args) {
        Application.Quit();
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
