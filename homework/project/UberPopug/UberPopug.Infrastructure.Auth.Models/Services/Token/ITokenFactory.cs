namespace UberPopug.Infrastructure.AuthModels.Services
{
   internal interface ITokenFactory
   {
      string GenerateToken(int size);
   }
}
