﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BibliotecaEstoque.Model;
using Controle_de_Estoque.Model;

namespace BibliotecaEstoque.Database
{
    public class ProdutoDB
    {
        public string mensagem;
        public string tabela;
        public LocalDbClass db;

        public ProdutoDB()
        {
            tabela = "Produtos";
            db = new LocalDbClass();
            mensagem = "Conexão estabelecida";

        }

        public string Inserir(string id,string codigo,string marca,string modelo, string descricao, int quantidade)
        {
            string SQL = $"INSERT INTO Produtos (codigo, marca, modelo, descricao, quantidade) VALUES ('{codigo}','{marca}','{modelo}', '{descricao}', '{quantidade}')";
            try
            {
                mensagem = db.SQLCommand(SQL);
                
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }

            return mensagem;
        }

        public List<Produto> BuscarTodos()
        {
            List<Produto> produtos = new List<Produto>();
            string SQL = $"SELECT * FROM Produtos";
            try
            {
                DataTable dt = db.DataTable(SQL);
                foreach (DataRow row in dt.Rows)
                {
                    Produto produto = new Produto
                    {
                        Codigo = Convert.ToString(row["codigo"]),
                        Marca = Convert.ToString(row["marca"]),
                        Modelo = Convert.ToString(row["modelo"]),
                        Descricao = Convert.ToString(row["descricao"]),
                        Quantidade = Convert.ToInt32(row["quantidade"])
                    };
                    produtos.Add(produto);
                }
                mensagem = "Busca realizada com sucesso";
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }

            return produtos;
        }

        public Produto BuscarPorCodigo(string codigo)
        {
            Produto produto = null;
            string SQL = $"SELECT * FROM Produtos WHERE codigo = '{codigo}'";
            try
            {
                DataTable dt = db.DataTable(SQL);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    produto = new Produto
                    {
                        Codigo = Convert.ToString(row["codigo"]),
                        Marca = Convert.ToString(row["marca"]),
                        Modelo = Convert.ToString(row["modelo"]),
                        Descricao = Convert.ToString(row["descricao"]),
                        Quantidade = Convert.ToInt32(row["quantidade"])
                    };
   
                    
                    mensagem = "Busca realizada com sucesso";
                }
                else
                {
                    mensagem = "Produto não encontrado";
                }
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }

            return produto;
        }

        public string Atualizar(Produto produto)
        {
            string SQL = $"UPDATE Produtos SET marca = '{produto.Marca}', modelo = '{produto.Modelo}', descricao = '{produto.Descricao}' WHERE codigo = '{produto.Codigo}'";
            try
            {
                mensagem = db.SQLCommand(SQL);
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }

            return mensagem;
        }
        public string EntrarProduto(Produto produto, int quantidadeEntrada) 
        {
            string SQL = $"UPDATE Produtos SET quantidade = quantidade + '{quantidadeEntrada}'  WHERE codigo = '{produto.Codigo}'";
            try
            {
                mensagem = db.SQLCommand(SQL);
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }

            return mensagem;
        }
        public string SaidaProduto(Produto produto, int quantidadeSaida)
        {
            string SQL = $"UPDATE Produtos SET quantidade = quantidade - '{quantidadeSaida}' WHERE codigo = '{produto.Codigo}' AND quantidade - '{quantidadeSaida}' >= 0";
            try
            {
                mensagem = db.SQLCommand(SQL);
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }

            return mensagem;
        }
        public string Remover(string codigo)
        {
            string SQL = $"DELETE FROM Produtos WHERE codigo = '{codigo}'";
            try
            {
                mensagem = db.SQLCommand(SQL);
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }

            return mensagem;
        }
        public string AdicionarNoRelatorio(string codigo, int quantidade, DateTime date, string tipoOperacao) 
        {

            string SQL = $"INSERT INTO Relatorio (data, codigo_produto, tipo, quantidade) VALUES ('{date.ToString("yyyy-MM-dd")}','{codigo}','{tipoOperacao}', {quantidade})";
            try
            {
               db.SQLCommand(SQL);
                mensagem = "Comando executado.";
            }
            catch 
            {
                mensagem = "Erro ao adicionar no relatório";
            }
            return mensagem;
        }
        public List<Relatorio> BuscarRelatorio(DateTime data)
        {
            List<Relatorio> relatorios = new List<Relatorio>();
            string SQL = $"SELECT * FROM Relatorio WHERE data = '{data.ToString("yyyy-MM-dd")}'";
            try
            {
                DataTable dt = db.DataTable(SQL);
                foreach (DataRow row in dt.Rows)
                {
                    Relatorio relatorio = new Relatorio
                    {
                        data = Convert.ToDateTime(row["data"]),
                        codigo = Convert.ToString(row["codigo_produto"]),
                        quantidade = Convert.ToInt32(row["quantidade"]),
                        tipoOperacao = Convert.ToString(row["tipo"])
                    };
                    relatorios.Add(relatorio);
                }
                mensagem = "Busca realizada com sucesso";
                
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }
            return relatorios;
        }
    }
}
