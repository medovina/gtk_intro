using Cairo;
using Gdk;
using Gtk;
using Key = Gdk.Key;
using Timeout = GLib.Timeout;

class Game {
    public int player_x = 200, player_y = 400;
    int dy = 0;     // vertical velocity in pixels/tick

    public void tick(bool move_left, bool move_right) {
        if (move_left)
            player_x -= 5;
        else if (move_right)
            player_x += 5;

        player_y += dy;
        if (player_y >= 400) {  // hit the ground
            dy = 0;     // stop falling
            player_y = 400;
        }
        else
            dy += 2;    // accelerate downward
    }

    public void jump() {
        if (player_y == 400 && dy == 0)
            dy = -20;
    }
}

class View : DrawingArea {
    Game game;
    ImageSurface player = new ImageSurface("player.png");

    public View(Game game) {
        this.game = game;
    }

    protected override bool OnDrawn (Context c) {
        c.SetSourceRGB(0, 0, 0);
        c.MoveTo(0, 400);
        c.LineTo(600, 400);
        c.Stroke();

        c.SetSourceSurface(player, game.player_x, game.player_y - player.Height);
        c.Paint();

        return true;
    }
}

class MyWindow : Gtk.Window {
    Game game = new Game();
    HashSet<Key> keys = new HashSet<Key>();

    public MyWindow() : base("game") {
        Resize(600, 600);
        Add(new View(game));
        Timeout.Add(30, on_timeout);    // 1 tick per 30 ms
    }

    bool on_timeout() {
        game.tick(keys.Contains(Key.Left), keys.Contains(Key.Right));
        QueueDraw();
        return true;
    }

    protected override bool OnKeyPressEvent(EventKey e) {
        if (e.Key == Key.space)
            game.jump();
        else
            keys.Add(e.Key);

        return true;
    }
    
    protected override bool OnKeyReleaseEvent(EventKey e) {
        keys.Remove(e.Key);
        return true;
    }

    protected override bool OnDeleteEvent(Event ev) {
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
