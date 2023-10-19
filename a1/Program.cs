namespace a1;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("starting Eto.Forms Gtk UI...");
        Run_Eto_Gtk(args);
    }

    [STAThread]
    public static void Run_Eto_Gtk(string[] args)
    {
        new Eto.Forms.Application(Eto.Platforms.Gtk).Run(new eto1.MainForm());
    }


}


