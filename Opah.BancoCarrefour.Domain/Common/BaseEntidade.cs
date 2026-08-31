using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opah.BancoCarrefour.Domain.Common;

public class BaseEntidade
{
    #region métodos

    public int CompareTo(BaseEntidade? other) => other == null ? 1 : other!.Id.CompareTo(Id);

    #endregion

    #region propriedades

    public Guid Id { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public DateTime? DataAtualizacao { get; set; } = null;

    #endregion
}
