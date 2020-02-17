using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_Lavanderia_Morden_UI.DAO;

namespace Project_Lavanderia_Morden_UI
{
    public partial class Form_Alterar_Cliente : MetroFramework.Forms.MetroForm
    {
        public Form_Alterar_Cliente(String nome, String telefone, String endereco, String email, String bairro, String cidade, String estado, String cep, int idCliente)
        {
            InitializeComponent();

            Label_IdCliente.Text = idCliente.ToString();
            TextBox_Nome.Text = nome;
            MaskedTextBox_Telefone.Text = telefone;
            TextBox_Endereco.Text = endereco;
            TextBox_Email.Text = email;
            TextBox_Bairro.Text = bairro;
            TextBox_Cidade.Text = cidade;
            TextBox_Estado.Text = estado;
            MaskedTextBox_Cep.Text = cep;
        }

        Conexao conexao = new Conexao();

        private void MaskedTextBox_Cep_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Correios.AtendeClienteClient consulta = new Correios.AtendeClienteClient("AtendeClientePort");

            if (e.KeyData == Keys.Tab && MaskedTextBox_Cep.Focused)
            {
                try
                {
                    var resultado = consulta.consultaCEP(MaskedTextBox_Cep.Text);

                    if (resultado != null)
                    {
                        TextBox_Bairro.Text = resultado.bairro;
                        TextBox_Cidade.Text = resultado.cidade;
                        TextBox_Endereco.Text = ($"{resultado.end} {resultado.complemento2}");
                        TextBox_Estado.Text = resultado.uf;
                    }
                }
                catch (Exception)
                {

                    TextBox_Bairro.Text = "";
                    TextBox_Estado.Text = "";
                    TextBox_Endereco.Text = "";
                    TextBox_Estado.Text = ""; ;
                }
            }
        }

        private void Button_Alterar_Click(object sender, EventArgs e)
        {

                conexao.Update(TextBox_Nome.Text.ToUpper(), MaskedTextBox_Telefone.Text, TextBox_Endereco.Text , TextBox_Email.Text, TextBox_Bairro.Text, TextBox_Cidade.Text , TextBox_Estado.Text, MaskedTextBox_Cep.Text, Convert.ToInt32(Label_IdCliente.Text));

            MessageBox.Show("Dados Alterados com sucesso !!!");
        }

        private void Button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
