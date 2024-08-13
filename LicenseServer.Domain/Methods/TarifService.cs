using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;

namespace LicenseServer.Domain.Methods
{
    public class TarifService
	{

		public async Task<HTTPResult<string>> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				var errorResult = Validator.TarifValidation(tarif);

				if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

                var currentTarif = DataGetter.TarifAPIToTarifEntity(tarif);

				await DataManager.AddEntityAsync(currentTarif);

                return HttpResults.StringResult.Success("Тариф создан успешно"); 
			}
			catch
			{
                return HttpResults.StringResult.Fail("Ошибка");
            }
		}

		public async Task<HTTPResult<List<TarifAPI.TarifResponse>>> GetAllTarifs()
		{
			try
			{
				var tarifs = await DataGetter.ListTarifAPI();

                return HttpResults.TarifsResult.Success(tarifs);
            }
			catch 
			{
                return HttpResults.TarifsResult.Fail("Ошибка");
            }
		}

		public async Task<HTTPResult<TarifAPI.TarifResponse>> GetTariffById(int tarifId)
		 {
			try
			{
				var idErrors = Validator.DataValidation(tarifId, "Не корректный Id тарифа");
                if (idErrors.Any())
                    return HttpResults.TarifResult.Fails(idErrors);

                var currentTarif = await DataGetter.TarifById(tarifId);

                return HttpResults.TarifResult.Success(currentTarif);
            }
			catch
			{
                return HttpResults.TarifResult.Fail("Ошибка");
            }
		}
	}
}
