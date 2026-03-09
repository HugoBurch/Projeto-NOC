using System;
using System.Collections.Generic;
using System.Text;

namespace NOC
{
    public class Tecnico
    {
        private int idTecnico;
        private string nome;      
        public Tecnico(int idTecnico, string nome )
        {
            this.idTecnico = idTecnico;
            this.nome = nome;            
        }
        public string getNome()
        {
            return nome;
        }
        public int getIdTecnico()
        {
            return idTecnico;
        }
        public void setId(int id)
        {
            this.idTecnico = id;
        }
        public void setNome( string nome)
        {
            this.nome = nome;
        }
    }
}
