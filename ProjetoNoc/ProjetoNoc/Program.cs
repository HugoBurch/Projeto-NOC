
using System;
using System.Collections.Generic;

using NOC;


namespace NOC
{

    public class Program
    {   
        private Cadastrar chamados;
        
        public Program()
        {   //instanciando a classe 
            this.chamados = new Cadastrar();
           
            List<RDOs> listaChamado = chamados.ObterChamados();
            List<Tecnico> listaTecnico = chamados.ObterTecnicos();
             
        }
        public void Executar()
        {   //executar o programa e tratamento de erro 
            int opcao = 1;
            do
            {   
                Menu();
                Console.WriteLine("Digite a opção desejada: ");
                while (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida. Por favor, insira um número inteiro.");
                }

                ExecutarMenu(opcao);
            }
            while (opcao != 7);
        }
        //criar RDO
        public void AdicionarChamados()
        {
            
            string descricao;
            string prioridade;
            string nome;
            int idTec;
            //TimeZone é utilizado para pegar o horaio da região 
            TimeZoneInfo fusoSaoPaulo = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            DateTime dataCriacao = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fusoSaoPaulo);
            DateTime? dataFinalizacao = null;

            Console.WriteLine("Tipo da RDO:");
            Console.WriteLine("[1]Rompimento - [2]Backbone - [3]CTO - [4]CEO ");
            Tipos tipo = Enum.Parse<Tipos>(Console.ReadLine());
            Console.Clear();
            Console.Clear();
            Console.WriteLine($"Selecione o Subtipo para {tipo}:");

            // Variável declarada fora para resolver o erro de escopo que você teve
            TipoComSubtipo tipoFinal = null;

            

            if (TipoSubtipoRelacao.Relacao.TryGetValue(tipo, out var subtiposPermitidos))
            {
                // Exibe apenas os subtipos que pertencem àquele Tipo
                foreach (var st in subtiposPermitidos)
                {
                    Console.WriteLine($"[{(int)st}] {st}");
                }
                SubTipos subTipoSelecionado;
                // Lê a opção e já valida se ela existe no HashSet
                while (true)
                {
                    Console.Write("Digite o número do subtipo: ");

                    string entrada = Console.ReadLine();

                    // verifica se digitou um número
                    if (!int.TryParse(entrada, out int numero))
                    {
                        Console.WriteLine("Digite apenas números.");
                        continue;
                    }

                    // converte número para enum
                    if (!Enum.IsDefined(typeof(SubTipos), numero))
                    {
                        Console.WriteLine("Subtipo não existe.");
                        continue;
                    }

                    subTipoSelecionado = (SubTipos)numero;

                    // verifica se pertence ao tipo escolhido
                    if (!subtiposPermitidos.Contains(subTipoSelecionado))
                    {
                        Console.WriteLine("Esse subtipo não pertence ao tipo selecionado.");
                        continue;
                    }

                    break;
                }
                tipoFinal = new TipoComSubtipo(tipo, subTipoSelecionado);

                Console.WriteLine("Tipo e Subtipo selecionados com sucesso.");
            }


            Console.WriteLine("Descrição da RDO");
            descricao = Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Prioridade da RDO (Baixa, Média, Alta)");
            prioridade = Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Qual o ID do técnico");
            idTec = int.Parse(Console.ReadLine());
            var tec = chamados.BuscarTecnico(idTec);
            Console.WriteLine("ID digitado: " + idTec);
            if (tec != null && tipoFinal != null)
            {
                int opcao;
                Console.WriteLine($"nome: {tec.getNome()}");
                Console.WriteLine("Confirma Técnico");
                Console.WriteLine("[1] Sim");
                Console.WriteLine("[2] Não");
                Console.Write(":");

                opcao = int.Parse(Console.ReadLine());
                Console.Clear();
                if (opcao == 1)
                {
                    Console.WriteLine("Técnico confirmado");
                    int id = chamados.ObterProximoId(); // pegar ID
                    RDOs chamado = new RDOs(id, tipoFinal, descricao, prioridade, dataCriacao,dataFinalizacao, tec, StatusE.Andamento);
                    try
                    {
                        chamados.AdicionarChamado(chamado);
                        Console.WriteLine($" ------- RDO {id.ToString("D3")} CADASTRADA ------- ");
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine("Erro ao cadastrar RDO: " + e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Técnico não encontrado");
                }
            }
        }
        public void CadastrarTecnico() 
        {
            string nome;
            int idtecnico;
            Console.WriteLine("Qual o nome do técnico");
            nome = Console.ReadLine();
            Console.WriteLine("Qual o ID do técnico");
            idtecnico = int.Parse(Console.ReadLine());

            if (chamados.ExisteTecnico(idtecnico))
            {
                Console.WriteLine("Técnico não cadastrado verifica o ID ou nome");
                Console.Clear();
            }
            else
            {
                Tecnico tecnico = new Tecnico(idtecnico, nome);
                chamados.AdicionarTecnico(tecnico);
                Console.WriteLine("Técnico cadastrado com sucesso");
                Console.Clear();
            }
        }
        public void ConsultarTecnico()
        {
            Console.WriteLine();
            foreach (var tecnico in chamados.ObterTecnicos()) {
                Console.Write($"ID: {tecnico.getIdTecnico()} ");
                Console.WriteLine($"Nome: {tecnico.getNome()}");
            }

        }
        public void ConsultarChamadosAbertos()
        {
            int pagina = 0;
            int itensPorPagina = 3;
            while (true) {
                Console.Clear();
                var listaFiltrada = chamados.ObterChamados().Skip(pagina * itensPorPagina).Take(itensPorPagina);
                var lista = chamados.ObterChamados();
                Console.WriteLine("LISTA DE RDO");
                if (lista.Count == 0 || lista.Any(c => c.getStatus() == StatusE.Fechado)) // usando linq para consultar 
                {
                    Console.WriteLine("Não há RDO em Andamento");
                    return;
                }
                else
                {
                    foreach (RDOs chamado in listaFiltrada.Where(c => c.getStatus() == StatusE.Andamento))
                    {

                        Console.WriteLine("-------------------------------------------------");
                        Console.WriteLine($"RDO: {chamado.getId().ToString("D3")}");
                        Console.WriteLine($"Tipo: {chamado.tipo}");
                        Console.WriteLine($"Descrição: {chamado.getDescricao()}");
                        Console.WriteLine($"Prioridade: {chamado.getPrioridade()}");
                        Console.WriteLine($"Data de Criação: {chamado.getDataCriacao()}");
                        Console.WriteLine($"Técnico Responsável: {chamado.getTecnico().getNome()} (ID: {chamado.getTecnico().getIdTecnico()})");
                        Console.WriteLine($"Status: {chamado.getStatus()}");
                        Console.WriteLine("-------------------------------------------------");

                        Console.WriteLine($"\n TOTAL DE RDO {lista.Count()} | [N] Próxima página | [P] Página anterior | [S] Sair");

                        var tecla = Console.ReadKey(true).Key;

                        if (tecla == ConsoleKey.S)
                        {
                            return; // O 'return' encerra o MÉTODO inteiro, garantindo que ele saia.
                        }
                        else if (tecla == ConsoleKey.N)
                        {
                            if ((pagina + 1) * itensPorPagina < lista.Count())
                                pagina++;
                        }
                        else if (tecla == ConsoleKey.P)
                        {
                            if (pagina > 0)
                                pagina--;
                        }
                    }
                }

            }
        }
        public void ConsultarChamadosFechados()
        {
            var lista = chamados.ObterChamados();
            Console.WriteLine("RDOs finalizadas");
            if (lista.Any(c => c.getStatus() == StatusE.Fechado))
            {
                foreach (RDOs chamado in lista.Where(c => c.getStatus() == StatusE.Fechado))
                {

                    Console.WriteLine("---------------------------");
                    Console.WriteLine($"RDO: {chamado.getId().ToString("D3")}");
                    Console.WriteLine($"Título: {chamado.getTipo().ToString()}");
                    Console.WriteLine($"Descrição: {chamado.getDescricao()}");
                    Console.WriteLine($"Prioridade: {chamado.getPrioridade()}");
                    Console.WriteLine($"Data de Criação: {chamado.getDataCriacao()}");
                    Console.WriteLine($"Data de Finalização: {chamado.getDataFinalizacao()}");
                    Console.WriteLine($"Técnico Responsável: {chamado.getTecnico().getNome()} (ID: {chamado.getTecnico().getIdTecnico()})");
                    Console.WriteLine($"Status: {chamado.getStatus()}");
                    Console.WriteLine("---------------------------");
                }
            }
            else
            {
                Console.WriteLine("RDO não encontrada ");
            }
        }
        public void ConsultarTodosChamados()
        {
            Console.WriteLine("Consultandos RDOs...");
            foreach (RDOs chamado in chamados.ObterChamados())
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine($"RDO: {chamado.getId().ToString("D3")}");
                Console.WriteLine($"Título: {chamado.getTipo().ToString()}");
                Console.WriteLine($"Descrição: {chamado.getDescricao()}");
                Console.WriteLine($"Prioridade: {chamado.getPrioridade()}");
                Console.WriteLine($"Data de Criação: {chamado.getDataCriacao()}");
                Console.WriteLine($"Data de Finalização: {chamado.getDataFinalizacao()}");
                Console.WriteLine($"Técnico Responsável: {chamado.getTecnico().getNome()} (ID: {chamado.getTecnico().getIdTecnico()})");
                Console.WriteLine($"Status: {chamado.getStatus()}");
                Console.WriteLine("---------------------------");
            }
        }
        public void Modificar()
        {
            Console.WriteLine("Qual ID da RDO");

            int id = int.Parse(Console.ReadLine());

            foreach (var modificar in chamados.ObterChamados())
            {
                if (chamados.ExiteRDO(id) && modificar.getId() == id)
                {
                    int opcao;
                    Console.WriteLine($"Titulo: {modificar.getTipo().ToString()}");
                    Console.WriteLine($"Dia:  {modificar.getDataCriacao()}");
                    Console.WriteLine("Confirma RDO");
                    Console.WriteLine("[1] Sim ");
                    Console.WriteLine("[2] Não ");
                    opcao = int.Parse(Console.ReadLine());
                    if (opcao == 1)
                    {
                        Console.WriteLine("RDO Confirmada");
                        Console.WriteLine("---------------------------");
                        Console.WriteLine("selecione a opção desejada:");
                        Console.WriteLine("[2] Descrição");
                        Console.WriteLine("[3] Técnico");
                        Console.WriteLine("[4] Prioridade");
                        opcao = int.Parse(Console.ReadLine());
                        switch (opcao)
                        {
                            case 1:
                                
                                Console.WriteLine("Titulo Modificado");
                                Executar();
                                Console.Clear();
                                break;
                            case 2:
                                Console.WriteLine("Descrição nova:");
                                modificar.setDescricao(Console.ReadLine());
                                Console.WriteLine("Descrição Modificada");
                                Executar();
                                Console.Clear();
                                break;
                            case 3:
                                Console.WriteLine("Digite o ID do técnico:");
                                modificar.getTecnico().setId(int.Parse(Console.ReadLine()));
                                modificar.getTecnico().setNome(Console.ReadLine());
                                Executar();
                                Console.Clear();
                                break;
                            case 4:
                                modificar.setPrioridade(Console.ReadLine());
                                Console.WriteLine("Prioridade Modificada");
                                Executar();
                                Console.Clear();
                                break;

                            default:
                                Console.WriteLine("Opção inválida");
                                Executar();
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("RDO não confirmada");
                    }
                }
                else
                {
                    Console.WriteLine("RDO não encontrada");
                }

            }
        }
        private void Finalizar()
        {
            int id;
            //TimeZone é utilizado para pegar o horaio da região 
            TimeZoneInfo fusoSaoPaulo = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            DateTime horario = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fusoSaoPaulo);

            
            Console.WriteLine("Qual ID da RDO que deseja finalizar");
            id = int.Parse(Console.ReadLine());
            foreach (RDOs finalizar in chamados.ObterChamados())
            {
                if (chamados.ExiteRDO(id) && finalizar.getStatus().Equals(StatusE.Andamento))
                {
                    Console.WriteLine($"Titulo {finalizar.getTipo().ToString()}");
                    Console.WriteLine($"Descrição {finalizar.getDescricao()}");
                    Console.WriteLine("Confirma RDO");
                    Console.WriteLine("[1] Sim ");
                    Console.WriteLine("[2] Não ");
                    int opcao = int.Parse(Console.ReadLine());
                    Console.Clear();
                    if (opcao == 1)
                    {
                        finalizar.setStatus(StatusE.Fechado);
                        finalizar.setDataFinalizacao(horario);
                        Console.WriteLine("RDO Finalizada");
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma RDO não encontrada");
                    }
                }
            }
        }
        private void Menu()
        {

            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║            MENU PRINCIPAL            ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 1  - Cadastrar RDO                   ║");
            Console.WriteLine("║ 2  - Consultar RDO Aberta            ║");
            Console.WriteLine("║ 3  - Consultar RDO Fechada           ║");
            Console.WriteLine("║ 4  - Consultar RDOs                  ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 5  - Cadastrar Técnico               ║");
            Console.WriteLine("║ 6  - Consultar Técnico               ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 7  - Modificar RDO                   ║");
            Console.WriteLine("║ 8  - Finalizar RDO                   ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 9  - Sair                            ║");
            Console.WriteLine("╚══════════════════════════════════════╝");

            Console.Write("\nDigite a opção desejada: ");
        }
        private void ExecutarMenu(int opcao)
        {
            switch (opcao)
            {
                case 1:
                    AdicionarChamados();
                    break;
                case 2:
                    ConsultarChamadosAbertos();
                    break;
                case 3:
                    ConsultarChamadosFechados();
                    break;
                case 4:
                    ConsultarTodosChamados();
                    break;
                case 5:
                    CadastrarTecnico();
                    break;
                case 6:
                    ConsultarTecnico();
                    break;
                case 7:
                    Modificar();
                    break;
                case 8:
                    Finalizar();
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
            }
        }
        public static void Main(string[] args)
        {   //CHAMA O PROGRAMA
            Program sistema = new Program();
            sistema.Executar();
        }
    }
}