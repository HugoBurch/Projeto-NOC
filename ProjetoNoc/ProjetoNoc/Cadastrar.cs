using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace NOC
{
    public class Cadastrar
    {
        public List<RDOs> listaChamados = new List<RDOs>();
        public List<Tecnico> listaTecnicos = new List<Tecnico>();

        public void AdicionarChamado(RDOs chamado)
        {
            this.listaChamados.Add(chamado);
        }
        public List<RDOs> ObterChamados()
        {
            return listaChamados;
        }
        public int ObterProximoId()
        {
            return listaChamados.Count + 1;
        }
        public void AdicionarTecnico(Tecnico tecnico)
        {
            this.listaTecnicos.Add(tecnico);
        }
        public List<Tecnico> ObterTecnicos()
        {
            return listaTecnicos;
        }
        public bool ExiteRDO(int id)
        {
            foreach (var chamado in listaChamados)
            {
                if (chamado.getId() == id)
                {
                    return true;
                }
            }
            return false;
        }
        public Tecnico BuscarTecnico(int id)
        {
            foreach (var tecnico in listaTecnicos)
            {
                if (tecnico.getIdTecnico().Equals(id))
                {
                    return tecnico;
                }
            }
            return null;
        }
        public bool ExisteTecnico(int idtecnico)
        {
            foreach (var tecnico in listaTecnicos)
            {
                if (tecnico.getIdTecnico().Equals(idtecnico))
                {
                    return true;
                }
            }
            return false;
        }
        public void DataFinalizacao(DateTime dataFinalizacao)
        {


        }
    }
}
