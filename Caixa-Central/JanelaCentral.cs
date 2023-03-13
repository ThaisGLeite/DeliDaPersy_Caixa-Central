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
        Usuario usuario;
        public JanelaCentral(string nome)
        {
            InitializeComponent();
            usuario = new(nome);

        }
    }
}
