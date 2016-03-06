using SaucierLibrary.ClienteBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaucierProjectTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Teste();
        }

        private void Teste()
        {
            Usuario user = Logar();
        }

        private Usuario Logar()
        {
            return Usuario.ValidarLoginSenha("login", "OigaleaH1@3", new Guid("B8F21383-8834-486C-98E4-2C09491565FD"));
        }
    }
}
