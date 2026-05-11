using System.ComponentModel.DataAnnotations;
using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Entities.Dtos.Internal;

public sealed record UpdateStoreOperatorStatusRequest([property: Required] StoreOperatorStatus Status);
