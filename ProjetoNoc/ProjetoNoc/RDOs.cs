using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Data;



namespace NOC
{
    public class RDOs
    {
        private int id;
        private string titulo;
        private string descricao;
        private string prioridade;
        public StatusE Status { get; set; }
        private DateTime dataCriacao;
        private DateTime? dataFinalizacao;
        private Tecnico tecnico;
        public RDOs(int id, string titulo, string descricao, string prioridade, DateTime dataCriacao,DateTime? dataFinalizacao, Tecnico tecnico, StatusE Status)
        {
            this.id = id;
            this.titulo = titulo;
            this.descricao = descricao;
            this.prioridade = prioridade;
            this.dataCriacao = dataCriacao;
            this.dataFinalizacao = dataFinalizacao;
            this.tecnico = tecnico;
            this.Status = Status;
        }
        public int getId()
        {
            return id;
        }
        public string getTitulo()
        {
            return titulo;
        }
        public string getDescricao()
        {
            return descricao;
        }
        public string getPrioridade()
        {
            return prioridade;
        }
        public DateTime getDataCriacao()
        {
            return dataCriacao;
        }
        public DateTime? getDataFinalizacao()
        {
            return dataFinalizacao;
        }
        public Tecnico getTecnico()
        {
            return tecnico;
        }
        public StatusE getStatus()
        {
            return Status;
        }
        public void setDataFinalizacao(DateTime dataFinalizacao)
        {
            this.dataFinalizacao = dataFinalizacao;
        }

        public void setStatus(StatusE status)
        {
            this.Status = status;
        }
        public void setPrioridade(string prioridade)
        {
            this.prioridade = prioridade;
        }
        public void setDescricao(string descricao)
        {
            this.descricao = descricao;
        }
        public void setTitulo(string titulo)
        {
            this.titulo = titulo;
        }
        public void setTecnico(Tecnico tecnico)
        {
            this.tecnico = tecnico;
        }

    }    
}
