namespace 密码生成器_WinForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Try to ensure console output uses UTF-8 so Rider's Run window displays Chinese paths correctly.
            // In a GUI (WinForms) process there may be no attached console and setting console encoding
            // can throw an IOException; wrap in try/catch and ignore failures harmlessly.
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                // Only attempt to set encodings; if no console is attached this may throw and will be ignored.
                System.Console.OutputEncoding = System.Text.Encoding.UTF8;
                System.Console.InputEncoding = System.Text.Encoding.UTF8;
            }
            catch (System.IO.IOException)
            {
                // No console available or windows API not supported — nothing actionable for GUI app.
            }
            catch (System.PlatformNotSupportedException)
            {
                // Some platforms may not support code page providers; ignore.
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}