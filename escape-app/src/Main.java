import uk.co.caprica.vlcj.player.component.EmbeddedMediaPlayerComponent;
import javax.swing.*;
import java.awt.*;



public class Main
{
    public static void main(String[] args)
    {
        EmbeddedMediaPlayerComponent component = new EmbeddedMediaPlayerComponent();

        JFrame f = new JFrame();
        f.setContentPane(component);
        f.setLocation(100, 100);
        f.setSize(1000, 600);
        f.setVisible(true);
        f.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);


        Canvas c = new Canvas();
        c.setBackground(Color.BLACK);
        JPanel p = new JPanel();
        p.setLayout(new BorderLayout());
        JLabel l = new JLabel();
        l.setIcon(new ImageIcon("assets/images/image.png"));
        p.add(c);
        p.add(l);
        f.add(p);

        component.mediaPlayer().media().play("assets/videos/video.mp4");

    }
}
