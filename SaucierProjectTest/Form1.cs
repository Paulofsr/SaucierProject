//using PessoalLibrary.Configuracoes;
using SaucierLibrary.CaixaBase;
using SaucierLibrary.ClienteBase;
using SaucierLibrary.FuncionarioBase;
using SaucierLibrary.ItemBase;
using SaucierLibrary.PagamentoBase;
using SaucierLibrary.RestauranteBase;
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
        Usuario _user;
        string _login = "login";
        string _senha = "123";
        Guid _clienteId = new Guid("637169bd-004b-44f9-92f7-9721d97d9297");
        Guid _skol = new Guid("95EF2BDC-DD9B-4C36-B120-A7F0608297CA");
        Guid _suco = new Guid("EEF2D1BC-3327-43C9-8273-08D36561AAA7");
        Guid _skolLata = new Guid("22E3F48A-20A0-49E7-B0D3-F3B045EDF084");
        Guid _debito = new Guid("8BAEC9F9-976E-4C89-A0A5-0C9DBB62679A");
        Guid _sobremesa = Guid.Empty;
        Caixa _caixa = Caixa.Empty();
        Comanda _comanda;

        public Form1()
        {
            InitializeComponent();
            Teste();
        }

        private void Teste()
        {
            //Cadastrar();
            //CadastrarTipoItem();
            CadastrarTipoPagamento();
            //LogarUsuario();
            //CadastrarRestaurante();
            //CadastrarTipoFuncionario("Administrador");
            //CadastrarTipoFuncionario("Caixa");
            //CadastrarTipoFuncionario("Garçon");
            //CadastrarFuncionario(_user);
            //Funcionario funcionario = Funcionario.Get(new FuncionarioCriteriaBase(new Guid("59FEE2F7-7B5E-4E72-B39E-49BEF9FB0E4A")));
            //CadastrarItem();
            //AbrirCaixa();
            //AddComanda();
            //AddItem();
            //AddPagamento();
            //FecharCaixa();
        }

        private void CadastrarTipoPagamento()
        {
            //AddTipoPagamento("Dinheiro");
            //AddTipoPagamento("Débito");
            //AddTipoPagamento("Crédito");
            //AddTipoPagamento("Cheque");
            //AddTipoPagamento("Vale Refeição");
            AddTipoPagamento("Desconto");
        }

        private void AddTipoPagamento(string tipo)
        {
            TipoPagamento tp = TipoPagamento.New(new TipoPagamentoCriteriaCreateBase());
            tp.Tipo = tipo;
            tp.Save();
        }

        private void AddPagamento()
        {
            _comanda.AddPagamento(_user.Id, _debito, 15, 15, 0);
        }

        private void AddItem()
        {
            _comanda.AddItem(_skolLata, _user.Id, 1);
        }

        private void AddComanda()
        {
            AddComanda("Mesa 1");
            _comanda = AddComanda("Mesa 2");
        }

        private Comanda AddComanda(string info)
        {
            Comanda comanda = Comanda.Empty();
            _caixa.AddComanda(ref comanda, info);
            return comanda;
        }

        private void FecharCaixa()
        {
            if (_caixa.FecharCaixa())
                Mensagem("Fechou Caixa");
            else
                Mensagem("Caixa não pode fechar");
        }

        private void Mensagem(string mensagem)
        {
            System.Console.WriteLine(mensagem);
        }

        private void CadastrarRestaurante()
        {
            Restaurante restaurante = Restaurante.New(new RestauranteCriteriaCreateBase());
            restaurante.CNPJ = "11111111";
            restaurante.Nome = "Meu Restaurante";
            restaurante.Save();
        }

        private void AbrirCaixa()
        {
            //CaixaSemConfiguracao();
            CaixaComConfiguracao();
        }

        private void CaixaComConfiguracao()
        {
            _caixa = Caixa.AbrirCaixa(_user, new List<SaucierLibrary.CaixaBase.Configuracao>());
            _caixa.AddConfiguracao(NewConfiguracao());
            Mensagem("Abriu Caixa com Configuração");
        }

        private Configuracao NewConfiguracao()
        {
            Configuracao configuracao = Configuracao.New(new ConfiguracaoCriteriaCreateBase(new List<ConfiguracaoItem>(), TipoConfiguracao.Consumo));
            //configuracao.
            return configuracao;
        }

        private void CaixaSemConfiguracao()
        {
            _caixa = Caixa.AbrirCaixa(_user, new List<SaucierLibrary.CaixaBase.Configuracao>());
            Mensagem("Abriu Caixa sem Configuração");
        }

        private void CadastrarItem()
        {
            AddItemSkol("Lata 269ml");
            AddItemSkol("Beats Extreme");
            AddItemSkol("Long Neck");
            AddItemSkol("Garrafa 600ml");
            AddItemSuco("Suco de Uva");
            AddItemSuco("Suco de Laranja");
            AddItemSuco("Suco de Limão");
        }

        private void AddItemSuco(string nome)
        {
            AddItem(nome, _suco);
        }

        private void AddItemSkol(string nome)
        {
            AddItem(nome, _skol);
        }

        private void AddItem(string nome, Guid tipo)
        {
            Item item = Item.New(new ItemCriteriaCreateBase());
            item.Custo = 10;
            item.Descricao = "Adicionado por teste.";
            item.Nome = nome;
            item.Preco = 15;
            item.TipoItemId = tipo;
            item.Save();
        }

        private void CadastrarTipoItem()
        {
            TipoItem bebida = AddTipoItem("Bebida", Guid.Empty);
            TipoItem wisky = AddTipoItem("Wisky", bebida.Id);
            AddTipoItem("Johnnie Walker", wisky.Id);
            AddTipoItem("White Horse", wisky.Id);
            TipoItem cerveja = AddTipoItem("Cerveja", bebida.Id);
            AddTipoItem("Backer", cerveja.Id);
            AddTipoItem("Skol", cerveja.Id);
            AddTipoItem("Cachaça", bebida.Id);
            AddTipoItem("Refrigerante", bebida.Id);
            AddTipoItem("Suco", bebida.Id);
        }

        private TipoItem AddTipoItem(string tipo, Guid parentId)
        {
            TipoItem tp = TipoItem.New(new TipoItemCriteriaCreateBase());
            tp.Tipo = tipo;
            tp.ParentId = parentId;
            tp.Save();
            return tp;
        }

        private void Cadastrar()
        {
            Cliente cliente = CadastrarCliente();
            CadastrarUsuario(cliente, _login, _senha);
        }

        private void CadastrarUsuario(Cliente cliente, string login, string senha)
        {
            Usuario usuario = Usuario.New(new UsuarioCriteriaCreateBase(cliente.Id));
            usuario.SetLogin(login);
            usuario.MudarSenha(string.Empty, senha);
            usuario.Nome = "Administrador";
            usuario.Email = "teste@teste.com.br";
            usuario.EmailConfirmado = true;
            usuario.Save();
            CadastrarFuncionario(usuario);
        }

        private void LogarUsuario()
        {
            _user = Usuario.ValidarLoginSenha(_login, _senha, _clienteId);
            PessoalLibrary.Configuracoes.Configuracao.BaseId = _user.Cliente.Base;
        }

        private void CadastrarFuncionario(Usuario usuario)
        {
            Funcionario funcionario = Funcionario.New(new FuncionarioCriteriaCreateBase(usuario.Id));
            funcionario.Ativo = true;
            funcionario.CPF = "111.111.111-11";
            funcionario.Nome = usuario.Nome;
            funcionario.RG = "MG-111";
            funcionario.TipoId = new Guid("67609A97-6AD0-4AF3-AA86-32DDFAF90703");
            funcionario.UsuarioId = usuario.Id;
            funcionario.Save();
        }

        private void CadastrarTipoFuncionario(string tipo)
        {
            TipoFuncionario tp = TipoFuncionario.New(new TipoFuncionarioCriteriaCreateBase());
            tp.Tipo = tipo;
            tp.Save();
        }

        private Cliente CadastrarCliente()
        {
            Cliente cliente = Cliente.New(new ClienteCriteriaCreateBase());
            cliente.Ativa = true;
            cliente.Nome = "Teste Geral";
            cliente.Save();
            return cliente;
        }
    }
}
