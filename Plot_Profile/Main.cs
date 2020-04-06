using CellToolDK;

public class Main
{
    private Transmiter t;
    private TifFileInfo fi;

    private void ApplyChanges()
    {
        //Apply changes and reload the image
        t.ReloadImage();
    }

    public void Input(TifFileInfo fi, Transmiter t)
    {
        this.t = t;
        this.fi = fi;
        //Main entrance
    }
}
