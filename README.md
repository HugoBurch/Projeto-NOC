# Projeto-NOC

Aplicação de console em **C# (.NET 10)** para registrar e acompanhar **RDOs** (chamados/ocorrências) de um **NOC** (Network Operations Center) de telecomunicações, com foco em ocorrências de rede óptica, como rompimentos, problemas em backbone, CTO e CEO.

O sistema oferece um menu interativo no terminal para cadastrar técnicos, abrir RDOs, consultá-las, modificá-las e finalizá-las.

> **Observação:** os dados são mantidos apenas em memória. Ao encerrar o programa, tudo o que foi cadastrado é perdido.

---

## Funcionalidades

- Cadastro de **técnicos** (ID e nome), com verificação de ID duplicado.
- Abertura de **RDO** com:
  - tipo e subtipo (validados conforme a relação entre eles);
  - descrição;
  - prioridade (Baixa, Média, Alta);
  - técnico responsável, com confirmação do nome;
  - data/hora de criação automática no fuso horário de São Paulo.
- Consulta de RDOs **abertas** (em andamento), com paginação de 3 itens por página.
- Consulta de RDOs **fechadas**.
- Consulta de **todas** as RDOs.
- Consulta da lista de técnicos.
- **Modificação** de RDO (descrição, técnico e prioridade).
- **Finalização** de RDO, com registro automático da data/hora de fechamento.

## Tipos e subtipos de RDO

| Tipo | Subtipos permitidos |
|---|---|
| Rompimento | Carga Alta, Ação de Terceiros |
| Backbone | Link, Carga Alta |
| CTO | Fibra Rompida Dentro da CTO, Fibra Torcida, CTO Danificada |
| CEO | CEO Danificada |

O programa só aceita um subtipo que pertença ao tipo escolhido.

## Status da RDO

| Status | Descrição |
|---|---|
| `Andamento` | Status inicial de toda RDO criada |
| `Atrasado` | Definido no enum, mas ainda não utilizado pelo fluxo atual |
| `Fechado` | Atribuído ao finalizar a RDO |

## Estrutura do projeto

```
Projeto-NOC/
├── README.md
└── ProjetoNoc/
    ├── ProjetoNoc.slnx          # Solução
    └── ProjetoNoc/
        ├── ProjetoNoc.csproj    # Projeto console (net10.0)
        ├── Program.cs           # Menu, entrada/saída e fluxo principal
        ├── Cadastrar.cs         # Armazenamento em memória (RDOs e técnicos) e buscas
        ├── RDOs.cs              # Modelo da RDO
        ├── Tecnico.cs           # Modelo do técnico
        ├── Tipo.cs              # Enums Tipos/SubTipos, relação entre eles e TipoComSubtipo
        └── Status.cs            # Enum StatusE
```

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download) ou superior.

Para conferir a versão instalada:

```bash
dotnet --version
```

## Como executar

```bash
# clonar o repositório
git clone https://github.com/HugoBurch/Projeto-NOC.git
cd Projeto-NOC/ProjetoNoc/ProjetoNoc

# compilar e executar
dotnet run
```

Também é possível abrir `ProjetoNoc/ProjetoNoc.slnx` no Visual Studio e executar o projeto.

## Como usar

Ao iniciar, o menu principal é exibido:

```
╔══════════════════════════════════════╗
║            MENU PRINCIPAL            ║
╠══════════════════════════════════════╣
║ 1  - Cadastrar RDO                   ║
║ 2  - Consultar RDO Aberta            ║
║ 3  - Consultar RDO Fechada           ║
║ 4  - Consultar RDOs                  ║
╠══════════════════════════════════════╣
║ 5  - Cadastrar Técnico               ║
║ 6  - Consultar Técnico               ║
╠══════════════════════════════════════╣
║ 7  - Modificar RDO                   ║
║ 8  - Finalizar RDO                   ║
╠══════════════════════════════════════╣
║ 9  - Sair                            ║
╚══════════════════════════════════════╝
```

**Fluxo recomendado:**

1. Cadastre ao menos um técnico (opção `5`).
2. Cadastre uma RDO (opção `1`): escolha o tipo, o subtipo, informe a descrição e a prioridade e confirme o técnico pelo ID.
3. Consulte as RDOs abertas (opção `2`). Na paginação, use `N` (próxima), `P` (anterior) e `S` (sair).
4. Finalize a RDO quando a ocorrência for resolvida (opção `8`).

## Tecnologias

- C# com .NET 10
- Aplicação de console
- LINQ para filtros e paginação
- `TimeZoneInfo` para registrar datas no horário de São Paulo

## Pontos de atenção e próximos passos

Alguns pontos observados no código que podem ser melhorados:

- **Persistência:** os dados ficam só em memória; adicionar banco de dados ou arquivo (JSON, SQLite etc.).
- **Saída do menu:** o menu indica `9 - Sair`, mas o laço principal em `Executar()` encerra com a opção `7`; a opção `9` também encerra via `Environment.Exit(0)`. Vale alinhar os dois.
- **Validação de entrada:** vários campos usam `int.Parse` e `Enum.Parse` diretamente, o que gera exceção se o usuário digitar um valor inválido; `TryParse` resolveria.
- **Prioridade:** hoje é texto livre; poderia ser um enum (Baixa, Média, Alta).
- **Status `Atrasado`:** previsto no enum, mas sem regra que o aplique.
- **Fuso horário:** o ID `"E. South America Standard Time"` é o identificador do Windows; em Linux/macOS pode ser necessário usar `"America/Sao_Paulo"`.
- **Repositório:** as pastas `bin/` e `obj/` estão versionadas; recomenda-se adicionar um `.gitignore` para projetos .NET e removê-las do controle de versão.

## Autor

[Hugo Burch](https://github.com/HugoBurch)
