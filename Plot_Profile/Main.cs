using CellToolDK;
using System.Windows.Forms;
using Plot_Profile;

public class Main
{
    private Transmiter t;
    private TifFileInfo fi;
    private MyForm myForm;

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
        if (fi == null || !fi.loaded)
        {
            MessageBox.Show("There is no loaded image!");
            return;
        }

        if (fi.sizeZ > 1 || fi.bitsPerPixel != 16 || fi.sizeC > 1)
        {
            MessageBox.Show("This plugin is currently avaliable only for 16 bit grayscale image time saries of one focal plane!");
            return;
        }

        if (this.myForm != null) myForm.Dispose();

        this.myForm = new MyForm(fi);
        this.myForm.Show();
        this.myForm.BringToFront();
    }
}
