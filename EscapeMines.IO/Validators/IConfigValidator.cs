using EscapeMines.Common;

namespace EscapeMines.IO.Validators
{
    public interface IConfigValidator
    {
        void SetConfig(GameConfig gameConfig);

        bool ValidateBoardDimensions();

        bool ValidateGameObjects();
    }
}
