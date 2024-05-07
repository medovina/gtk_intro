using Gdk;
using Gtk;
using static Gtk.Orientation;

class MyWindow : Gtk.Window {
    Entry text_box = new Entry();
    Button generate_button = new Button("Generate");
    CheckButton uppercase = new CheckButton("Uppercase");
    Button exit_button = new Button("Exit");

    public MyWindow() : base("password selector") {
        Box row = new Box(Horizontal, 0);
        row.Add(new Label("password: "));
        row.Add(text_box);

        Box row2 = new Box(Horizontal, 3);
        row2.Add(generate_button);
        generate_button.Clicked += on_generate;
        row2.Add(uppercase);

        Box row3 = new Box(Horizontal, 0);
        row3.Add(exit_button);
        exit_button.Clicked += on_exit;

        Box vbox = new Box(Vertical, 5);
        vbox.Add(row);
        vbox.Add(row2);
        vbox.Add(row3);
        Add(vbox);
        vbox.Margin = 5;
    }

    void on_generate(object? sender, EventArgs args) {
        Random rand = new Random();
        string consonants = "bcdfghjklmnoqrstuwxyz";
        string vowels = "aeiou";
        string password = "";
        for (int i = 0 ; i < 5 ; ++i) {
            password += consonants[rand.Next(consonants.Length)];
            password += vowels[rand.Next(vowels.Length)];
        }
        if (uppercase.Active)
            password = password.ToUpper();
        text_box.Text = password;
    }

    void on_exit(object? sender, EventArgs args) {
        using (MessageDialog d = new MessageDialog(this,
            DialogFlags.Modal, MessageType.Warning, ButtonsType.Ok,
            "the password is '{0}'", text_box.Text)) {
            d.Run();
            Application.Quit();
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
