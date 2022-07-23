using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GestorDeClientes
{
    internal class Program
    {
        [System.Serializable] //vamos salvar os clientes em arquivo
        struct Cliente // tipo principal
        {
            public string nome;
            public string email;
            public string cpf; 
        }

        static List<Cliente> clientes = new List<Cliente>(); //vamos salvar os clientes aqui


        enum Menu { Listagem = 1, Adicionar, Remover, Sair }
        static void Main(string[] args)
        {
            Carregar();
            bool escolheuSair = false;
            while (!escolheuSair)
            {
                Console.WriteLine("Sistema de Clientes!");
                Console.WriteLine("[1] - Listagem\n[2] - Adicionar\n[3] - Remover\n[4] - Sair");
                int intOp = int.Parse(Console.ReadLine());
                Menu opcao = (Menu)intOp; //convertendo var int para menu e armazenando na var opcao do tipo menu

                switch (opcao)
                {
                    case Menu.Listagem:
                        Listar();
                        break;

                    case Menu.Adicionar:
                        Adicionar();
                        break;

                    case Menu.Remover:
                        Remover();
                        break;

                    case Menu.Sair:
                        escolheuSair = true;
                        break;

                    default:
                        break;
                }
                Console.Clear();
            }
        }

        static void Adicionar()
        {
            Cliente cliente = new Cliente();
            Console.WriteLine("Cadastro de cliente: ");
            Console.WriteLine("Nome do cliente: ");
            cliente.nome = Console.ReadLine();

            Console.WriteLine("Email do cliente: ");
            cliente.email = Console.ReadLine();

            Console.WriteLine("CPF do cliente: ");
            cliente.cpf = Console.ReadLine();

            clientes.Add(cliente);
            Salvar();

            Console.WriteLine("Cadastro concluido, aperte enter para sair.");
            Console.ReadLine();
        }

        static void Listar()
        {
            if (clientes.Count() > 0) //Se tem pelo menos um cliente cadastrado
            {
                Console.WriteLine("Lista de Clientes: ");
                int i = 0;
                foreach (Cliente cliente in clientes)
                {
                    Console.WriteLine("=======================");
                    Console.WriteLine($"ID: {i}");
                    Console.WriteLine($"Nome: {cliente.nome}");
                    Console.WriteLine($"Email: {cliente.email}");
                    Console.WriteLine($"CPF: {cliente.cpf}");
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Nenhum cliente cadastrado!");
            }
            Console.WriteLine("=======================");
            Console.WriteLine("Apente enter para sair.");
            Console.ReadLine();
        }

        static void Remover()
        {
            Listar();
            bool idInvalido = false;
            while (idInvalido == false)
            {
                Console.WriteLine("Digite o ID do cliente que você quer remover: ");
                int id = int.Parse(Console.ReadLine());
                if (id >= 0 && id < clientes.Count)
                {
                    clientes.RemoveAt(id);
                    idInvalido = true;
                    Salvar(); //Sempre que ocorrer uma alteração, deve salvar.
                    Console.WriteLine("Cliente removido com sucesso!\nAperte enter para sair");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("ID inválido! Aperte enter para tentar novamente");
                    Console.ReadLine();
                }
            }
        }

        static void Salvar() 
        {
            //sua responsabilidade é salvar no disco rigido a lista atual
            FileStream stream = new FileStream("Clients.dat", FileMode.OpenOrCreate);
            BinaryFormatter encoder = new BinaryFormatter(); //Vamos usalo para salvar os dados em arquivo binário

            encoder.Serialize(stream, clientes);

            stream.Close();
        }

        static void Carregar()
        {
            FileStream stream = new FileStream("Clients.dat", FileMode.OpenOrCreate);

            try
            {
                BinaryFormatter encoder = new BinaryFormatter();

                clientes = (List<Cliente>)encoder.Deserialize(stream);

                if (clientes == null)
                {
                    clientes = new List<Cliente>();
                }
            }
            catch (Exception)
            {
                clientes = new List<Cliente>();
            }

            stream.Close();
        }
    }
}
