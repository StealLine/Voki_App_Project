using SharedKernel.common.vokis;
using SharedKernel.domain;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using SharedKernel.user_ctx;

namespace VokiTakingServicesLib.Domain.base_voki_aggregate;

public abstract class BaseVoki : AggregateRoot<VokiId>
{
    protected BaseVoki() { }
    public VokiName Name { get; }
    protected VokiManagersIdsSet ManagersSet { get; private set; }
    protected abstract BaseVokiInteractionSettings BaseInteractionSettings { get; }

    protected BaseVoki(VokiName name, VokiManagersIdsSet managers) {
        Name = name;
        ManagersSet = managers;
    }

    public ErrOrNothing CheckUserAccessToTake(IUserCtx userCtx) {
        if (BaseInteractionSettings.SignedInOnlyTaking && !userCtx.IsAuthenticated(out _)) {
            return ErrFactory.NoAccess("To take this Voki you need to be signed in");
        }

        return ErrOrNothing.Nothing;
    }

    public void UpdateManagersSet(VokiManagersIdsSet newManagers) =>
        ManagersSet = newManagers;
}