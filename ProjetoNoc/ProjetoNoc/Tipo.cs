using System;
using System.Collections.Generic;
using System.Text;

namespace NOC
{
    public enum Tipos : int
    {
        Rompimento = 1,
        Backbone = 2,
        CTO = 3,
        CEO = 4

    }
    public enum SubTipos : int
    {
        //Rompimento
        CargaAlta = 1,
        AcaoTerceiros = 2,
        //Backbone
        Link = 3,
        //CTO
        Fibra_Rompida_Dentro_da_CTO = 4,
        Fibra_Torcida = 5,
        CTO_Danificada = 6,
        //CEO
        CEO_Danificada = 7
    }
    public static class TipoSubtipoRelacao
    {
        // Usamos HashSet para buscas ultra-rápidas
        public static readonly Dictionary<Tipos, HashSet<SubTipos>> Relacao =
            new Dictionary<Tipos, HashSet<SubTipos>>
        {
        { Tipos.Rompimento, [SubTipos.CargaAlta, SubTipos.AcaoTerceiros] },
        { Tipos.Backbone,   [SubTipos.Link, SubTipos.CargaAlta] },
        { Tipos.CTO,        [SubTipos.Fibra_Rompida_Dentro_da_CTO, SubTipos.Fibra_Torcida, SubTipos.CTO_Danificada] },
        { Tipos.CEO,        [SubTipos.CEO_Danificada] }
        };

        public static bool SubtipoValido(Tipos tipo, SubTipos subtipo)
        {
            // Se encontrar o tipo, verifica se o subtipo está no conjunto; caso contrário, retorna false
            return Relacao.TryGetValue(tipo, out var subtipos) && subtipos.Contains(subtipo);
        }
    }
    public class TipoComSubtipo
    {
        public Tipos tipo { get; set; }
        public SubTipos subTipos { get; set; }

        public TipoComSubtipo(Tipos tipo, SubTipos subtipos)
        {
            this.tipo = tipo;
            this.subTipos = subtipos;
        }
        public override string ToString()
        {
            return $"{tipo} - {subTipos}";
        }
    }
}

