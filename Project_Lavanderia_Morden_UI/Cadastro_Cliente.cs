using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_Lavanderia_Morden_UI.DAO;

namespace Project_Lavanderia_Morden_UI
{
    public partial class Form_Cliente : MetroFramework.Forms.MetroForm
    {
        Conexao conexao = new Conexao();
        public Form_Cliente()
        {
            InitializeComponent();
        }

        private void Form_Cliente_Load(object sender, EventArgs e)
        {
            //Cliente
            DataGridView_Cliente.DataSource = conexao.SelectGrid("SELECT ID_CLIENTE,NOME as [Nome],DT_CADASTRO as [Data de Cadastro], TELEFONE AS Telefone, ENDERECO as [Endereço],EMAIL as [Email],BAIRRO as [Bairro],CIDADE as [Cidade],ESTADO as [UF],CEP FROM TB_CLIENTE ORDER BY ID_CLIENTE Desc");
            DataGridView_Cliente.Columns["ID_CLIENTE"].Visible = false;

            //Faturamento
            DataGridView_Faturamento.DataSource = conexao.SelectGrid("SELECT ID_FAT AS [Nº Faturamento],NOME AS Nome, DT_CADASTRO AS [Data de Cadastro], STATUS as Status FROM VW_FATURAMENTO");
        }

        private void TextBox_FiltroCliente_TextChanged(object sender, EventArgs e)
        {
            //Cliente
            if (TextBox_FiltroCliente.Text == "")
            {
                DataGridView_Cliente.DataSource = conexao.SelectGrid("SELECT * FROM TB_CLIENTE ORDER BY ID_CLIENTE DESC");
            }
            else
            {
                DataGridView_Cliente.DataSource = conexao.SelectGrid($"SELECT * FROM TB_CLIENTE WHERE NOME LIKE '%{TextBox_FiltroCliente.Text}%' ORDER BY NOME DESC");

            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            if (RadioButton_Novo.Checked)
            {
                conexao.Insert(TextBox_Nome.Text.ToUpper(), MaskedTextBox_Telefone.Text, TextBox_Endereco.Text, TextBox_Email.Text, TextBox_Bairro.Text, TextBox_Cidade.Text, TextBox_Estado.Text, MaskedTextBox_Cep.Text);
                DataGridView_Cliente.DataSource = conexao.SelectGrid("SELECT * FROM TB_CLIENTE ORDER BY ID_CLIENTE DESC");
                MessageBox.Show("Cadastrado com sucesso !!!");
                ClearFields();
            }
        }

        private void ClearFields()
        {
            TextBox_Estado.Clear();
            MaskedTextBox_Cep.Clear();
            TextBox_Cidade.Clear();
            TextBox_Bairro.Clear();
            TextBox_Nome.Clear();
            TextBox_Email.Clear();
            TextBox_Endereco.Clear();
            MaskedTextBox_Telefone.Clear();
            TextBox_Nome.Focus();
        }

        private void DataGridView_Cliente_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (RadioButton_Alterar.Checked && e.RowIndex != -1)
            {
                
                string nome = DataGridView_Cliente[1, e.RowIndex].Value.ToString();
                string telefone = DataGridView_Cliente[3, e.RowIndex].Value.ToString();
                string endereco = DataGridView_Cliente[4, e.RowIndex].Value.ToString();
                string email = DataGridView_Cliente[5, e.RowIndex].Value.ToString();
                string bairro = DataGridView_Cliente[6, e.RowIndex].Value.ToString();
                string cidade = DataGridView_Cliente[7, e.RowIndex].Value.ToString();
                string estado = DataGridView_Cliente[8, e.RowIndex].Value.ToString();
                string cep = DataGridView_Cliente[9, e.RowIndex].Value.ToString();
                int idCliente = Convert.ToInt32(DataGridView_Cliente[0, e.RowIndex].Value);
                Form_Alterar_Cliente frm = new Form_Alterar_Cliente(nome, telefone, endereco, email, bairro, cidade, estado, cep, idCliente);
                frm.ShowDialog();
               

            }
            else if (RadioButton_Excluir.Checked && e.RowIndex != -1)
            {
                int idCliente = Convert.ToInt32(DataGridView_Cliente["ID_CLIENTE", e.RowIndex].Value);

                if (MessageBox.Show("Deseja excluir o registro?", "Exclusão", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conexao.Delete(idCliente);
                    DataGridView_Cliente.DataSource = conexao.SelectGrid("SELECT * FROM TB_CLIENTE ORDER BY ID_CLIENTE DESC");
                    

                }
                MessageBox.Show("Excluido com sucesso !!!");
            }
            else if (RadioButton_Os.Checked && e.RowIndex != -1)
            {
                string nome = DataGridView_Cliente["NOME", e.RowIndex].Value.ToString();
                int idCliente = Convert.ToInt32(DataGridView_Cliente["ID_CLIENTE", e.RowIndex].Value);
                Form_OS form = new Form_OS(nome, idCliente);
                form.ShowDialog();
            }
        }

        private void Button_Fechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox_FiltroFaturamento_TextChanged(object sender, EventArgs e)
        {
            if (TextBox_FiltroFaturamento.Text == "")
            {
                DataGridView_Faturamento.DataSource = conexao.SelectGrid("SELECT * FROM VW_FATURAMENTO");
            }
            else
            {
                DataGridView_Faturamento.DataSource = conexao.SelectGrid($"SELECT * FROM VW_FATURAMENTO WHERE NOME LIKE '%{TextBox_FiltroFaturamento.Text}%'");
            }
        }

        private void Button_Filtrar_Click(object sender, EventArgs e)
        {
            var de = DateTimePicker_De.Value.Date;
            var ate = DateTimePicker_Ate.Value.Date;
            var sql = $"SELECT * FROM VW_FATURAMENTO WHERE DT_CADASTRO BETWEEN '{de}' AND '{ate}' ";
            DataGridView_Faturamento.DataSource = conexao.SelectGrid(sql);

            if (!string.IsNullOrEmpty(TextBox_FiltroFaturamento.Text))
            {
                sql += $" AND NOME LIKE '%{TextBox_FiltroFaturamento.Text}%'";
                DataGridView_Faturamento.DataSource = conexao.SelectGrid(sql);
            }
        }

        private void Calcular_Total()
        {
            decimal valorTotal = 0;

            foreach (DataGridViewRow col in DataGridView_OsF.Rows)
            {
                valorTotal += Convert.ToDecimal(col.Cells["Preço"].Value);

            }

            Label_Total.Text = valorTotal.ToString("C");
        }

        private void DataGridView_Faturamento_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int id_Fat = Convert.ToInt32(DataGridView_Faturamento[0, e.RowIndex].Value);
                DataGridView_OsF.DataSource = conexao.SelectGrid($"SELECT DESCRICAO AS Descrição, QUANTIDADE AS Quantidade, PRECO as Preço FROM TB_OS WHERE ID_FAT = {id_Fat}");
                Calcular_Total();
            }
           
        }

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

        private StringBuilder htmlMessageBody(DataGridView dg)
        {
            StringBuilder strB = new StringBuilder();

            //create html & table
            strB.AppendLine("<html><body>");
            strB.AppendLine("<link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css'");
            strB.AppendLine("integrity='sha384 - ggOyR0iXCbMQv3Xipma34MD + dH / 1fQ784 / j6cY / iJTQUOhcWr7x9JvoRxT2MZw1T' crossorigin='anonymous'>");
            strB.AppendLine("<div class='table table-sm'>");
            strB.AppendLine("<table class='table-bordered'>");
            strB.AppendLine("<thead class='thead-dark'>");
            strB.AppendLine("<tr>");
            //cteate table header
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                strB.AppendLine($"<th scope='col'>{dg.Columns[i].HeaderText}</th>");
            }
            //create table body
            strB.AppendLine("</tr></thead><tbody>");
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                strB.AppendLine("<tr>");
                foreach (DataGridViewCell dgvc in dg.Rows[i].Cells)
                {
                    strB.AppendLine($"<td>{dgvc.Value.ToString()}</td>");

                }
                strB.AppendLine("</tr>");

            }
            //table footer & end of html file
            strB.AppendLine("<tr><td colspan='2' style='text-align: center' ><b>TOTAL</b></td>");
            strB.AppendLine($"<td>{Label_Total.Text}");
            strB.AppendLine("</td></tr></tbody></table></div></body></html>");


            return strB;
        }
        private void EnviaEmails()
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("ivanilsonsantosoliveira@gmail.com");
            mail.To.Add("ivanilsonzica@hotmail.com"); // para
            mail.Subject = "Serviços"; // assunto
            mail.IsBodyHtml = true;
            mail.Body = htmlMessageBody(DataGridView_OsF).ToString(); // mensagem


            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.EnableSsl = true; // GMail requer SSL
                smtp.Port = 587;       // porta para SSL
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                smtp.UseDefaultCredentials = false;

                // seu usuário e senha para autenticação
                smtp.Credentials = new NetworkCredential("ivanilsonsantosoliveira@gmail.com", "ayyckacpnfekkrlu");

                // envia o e-mail
                smtp.Send(mail);
            }
        }

    }
}
