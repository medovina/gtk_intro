using Gdk;
using Gtk;

class MyWindow : Gtk.Window {
    static (string, string)[] capitals = {
        ("Austria", "Vienna"),
        ("the Czech Republic", "Prague"),
        ("Germany", "Berlin"),
        ("Hungary", "Budapest"),
        ("Slovakia", "Bratislava"),
    };

    ComboBoxText country_box = new ComboBoxText();
    ComboBoxText capital_box = new ComboBoxText();

    public MyWindow() : base("capitals") {
        foreach ((string country, string capital) in capitals) {
            country_box.AppendText(country);
            capital_box.AppendText(capital);
        }

        Box hbox = new Box(Orientation.Horizontal, 5);
        hbox.Add(new Label("The capital of"));
        hbox.Add(country_box);
        hbox.Add(new Label("is"));
        hbox.Add(capital_box);
        hbox.Margin = 8;
        Add(hbox);

        country_box.Changed += on_country_changed;
        capital_box.Changed += on_capital_changed;
    }

    void on_country_changed(object? sender, EventArgs args) {
        capital_box.Active = country_box.Active;
    }

    void on_capital_changed(object? sender, EventArgs args) {
        country_box.Active = capital_box.Active;
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
