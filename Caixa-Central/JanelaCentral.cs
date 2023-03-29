using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caixa_Central
{
    public partial class JanelaCentral : Form
    {
        readonly Usuario usuario;
        public JanelaCentral(string nome)
        {
            InitializeComponent();
            usuario = new(nome);
            this.Text = "Logado como: " + usuario.Nome;
        }

        private void JanelaCentral_FormClosed(object sender, FormClosedEventArgs e) => Application.Exit();

        private void TextBoxCadastroAssinantesSobreNome_TextChanged(object sender, EventArgs e)
        {
            if ((textBoxCadastroAssinantesNome.Text.Length > 3) && (textBoxCadastroAssinantesSobreNome.Text.Length > 3))
            {
                groupBoxCadastroAssinantesPlanoEscolhido.Visible = true;
            }
        }
    }
}
