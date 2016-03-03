using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace GameStore.WebUI.Components.AttributeAdapters
{
	public class LocalizedRequiredAttributeAdapter : RequiredAttributeAdapter
	{
		public LocalizedRequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
			: base(metadata, context, attribute)
		{
			if (attribute.ErrorMessageResourceType == null)
			{
				attribute.ErrorMessageResourceType = typeof(GlobalResource);
			}

			if (string.IsNullOrEmpty(attribute.ErrorMessageResourceName))
			{
				attribute.ErrorMessageResourceName = "RequiredAttribute_ValidationError";
			}
		}
	}
}