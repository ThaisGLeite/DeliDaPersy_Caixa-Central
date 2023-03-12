using Newtonsoft.Json;
using System.Text;


namespace Caixa_Central
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TextBoxPassWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string nome = textBoxNome.Text;
                if (nome.Length > 3)
                {
                    string senha = textBoxPassWord.Text;
                    if(senha.Length > 3)
                    {
                        Usuario usuario = new(nome, senha);
                        _ = Auxiliar.Login(nome, senha);


                    }
                }
            }
        }
    }
}