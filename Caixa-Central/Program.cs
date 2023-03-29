namespace Caixa_Central
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //Metodo padrão, substituir por esse quando for para produção
            //Application.Run(new Login());

            //Entra direto na janela de teste para não dar trabalho de logar toda vida
            Application.Run(new JanelaCentral("===Testes==="));
        }
    }
}