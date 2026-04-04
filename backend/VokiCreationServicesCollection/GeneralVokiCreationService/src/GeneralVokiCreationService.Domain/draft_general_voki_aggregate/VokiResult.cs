using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiResult : Entity<GeneralVokiResultId>
{
    private VokiResult() { }
    public VokiResultName Name { get; private set; }
    public VokiResultText Text { get; private set; }
    public GeneralVokiResultImageKey? Image { get; private set; }
    public DateTime CreationDate { get; } //to sort when showing to user

    private VokiResult(VokiResultName name, DateTime dateTime) {
        Id = GeneralVokiResultId.CreateNew();
        Name = name;
        Text = GeneralVokiPresets.GetRandomResultText();
        Image = null;
        CreationDate = dateTime;
    }

    public static VokiResult CreateNew(VokiResultName name, DateTime dateTime) => new(name, dateTime);

    public ErrOrNothing Update(VokiResultName name, VokiResultText text, GeneralVokiResultImageKey? newKey) {
        if (newKey is not null && newKey.ResultId != Id) {
            return ErrFactory.Conflict(
                "Provided image does not belong to the specified result",
                $"Result id: {Id}, key: {newKey}"
            );
        }

        this.Name = name;
        this.Text = text;
        Image = newKey;
        return ErrOrNothing.Nothing;
    }
}