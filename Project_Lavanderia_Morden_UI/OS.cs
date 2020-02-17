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
    public partial class Form_OS : MetroFramework.Forms.MetroForm
    {
        public Form_OS(String nome, int id)
        {
            InitializeComponent();
            Label_IdCliente.Text = id.ToString();
            Label_Cliente.Text = nome;
        }

        Conexao conexao = new Conexao();

        private void Button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Salvar_Click(object sender, EventArgs e)
        {
            var id_Fat = conexao.Select_Id_Faturamento();

            if (DataGridView_Os.Rows.Count > 1)
            {

                for (int i = 0; i <= DataGridView_Os.Rows.Count - 1; i++)
                {
                    if (DataGridView_Os.Rows[i].Cells["Total"].Value != null)
                    {

                        string descricao = DataGridView_Os.Rows[i].Cells["Descrição"].Value.ToString(); //Descricao 
                        int quantidade = Convert.ToInt32(DataGridView_Os.Rows[i].Cells["Quantidade"].Value); //Quantidade
                        decimal preco = Convert.ToDecimal(DataGridView_Os.Rows[i].Cells["Preço"].Value); //Preco
                        decimal total = Convert.ToDecimal(DataGridView_Os.Rows[i].Cells["Total"].Value); //Total
                        int idCliente = Convert.ToInt32(Label_IdCliente.Text);

                        conexao.InsertOs(descricao, quantidade, preco, total, idCliente, id_Fat);
                    }
                }

                DataGridView_Os.Rows.Clear();
                Label_Total.Text = "";
                MessageBox.Show("Cadastrado com sucesso !!!");
            }
        }

        private void DataGridView_Os_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    decimal quantidade = Convert.ToDecimal(DataGridView_Os.CurrentRow.Cells["Quantidade"].Value);
                    decimal preco = Convert.ToDecimal(DataGridView_Os.CurrentRow.Cells["Preço"].Value);

                    if (preco.ToString() != "" && quantidade.ToString() != "")
                    {
                        DataGridView_Os.CurrentRow.Cells["Total"].Value = preco * quantidade;
                    }
                }
                decimal valorTotal = 0;
                string valor = "";

                if (DataGridView_Os.CurrentRow.Cells[3].Value != null)
                {
                    valor = DataGridView_Os.CurrentRow.Cells["Total"].Value.ToString();

                    if (!valor.Equals(""))
                    {
                        for (int i = 0; i <= DataGridView_Os.RowCount - 1; i++)
                        {
                            if (DataGridView_Os.Rows[i].Cells["Total"].Value != null)
                                valorTotal += Convert.ToDecimal(DataGridView_Os.Rows[i].Cells["Total"].Value);
                        }

                        if (valorTotal == 0)
                        {
                            MessageBox.Show("Nenhum registro encontrado");
                        }

                        Label_Total.Text = valorTotal.ToString("C");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
