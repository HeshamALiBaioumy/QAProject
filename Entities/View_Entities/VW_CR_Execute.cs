using QA.Entities.Business_Entities;

namespace QA.Entities.View_Entities
{
    public class VW_CR_Execute
    {
        public Business_Entities.Ent_CR CR { get; set; }

        public Ent_MapSelection projectMapSelection { get; set; }
    }
}