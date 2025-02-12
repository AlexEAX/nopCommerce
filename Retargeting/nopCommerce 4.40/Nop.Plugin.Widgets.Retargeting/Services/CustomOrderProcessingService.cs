﻿using System;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Core.Events;
using Nop.Services.Affiliates;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Services.Tax;
using Nop.Services.Vendors;

namespace Nop.Plugin.Widgets.Retargeting.Services
{
    public class CustomOrderProcessingService : OrderProcessingService
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPluginService _pluginService;

        public CustomOrderProcessingService(
            IPluginService pluginService,
            CurrencySettings currencySettings,
            IAddressService addressService,
            IAffiliateService affiliateService,
            ICheckoutAttributeFormatter checkoutAttributeFormatter,
            ICountryService countryService,
            ICurrencyService currencyService,
            ICustomerActivityService customerActivityService,
            ICustomerService customerService,
            ICustomNumberFormatter customNumberFormatter,
            IDiscountService discountService,
            IEncryptionService encryptionService,
            IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            IGiftCardService giftCardService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILogger logger,
            IOrderService orderService,
            IOrderTotalCalculationService orderTotalCalculationService,
            IPaymentPluginManager paymentPluginManager,
            IPaymentService paymentService,
            IPdfService pdfService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeFormatter productAttributeFormatter,
            IProductAttributeParser productAttributeParser,
            IProductService productService,
            IRewardPointService rewardPointService,
            IShipmentService shipmentService,
            IShippingService shippingService,
            IShoppingCartService shoppingCartService,
            IStateProvinceService stateProvinceService,
            ITaxService taxService,
            IVendorService vendorService,
            IWebHelper webHelper,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings,
            OrderSettings orderSettings,
            PaymentSettings paymentSettings,
            RewardPointsSettings rewardPointsSettings,
            ShippingSettings shippingSettings,
            TaxSettings taxSettings) :
            base(
                currencySettings,
                addressService,
                affiliateService,
                checkoutAttributeFormatter,
                countryService,
                currencyService,
                customerActivityService,
                customerService,
                customNumberFormatter,
                discountService,
                encryptionService,
                eventPublisher,
                genericAttributeService,
                giftCardService,
                languageService,
                localizationService,
                logger,
                orderService,
                orderTotalCalculationService,
                paymentPluginManager,
                paymentService,
                pdfService,
                priceCalculationService,
                priceFormatter,
                productAttributeFormatter,
                productAttributeParser,
                productService,
                rewardPointService,
                shipmentService,
                shippingService,
                shoppingCartService,
                stateProvinceService,
                taxService,
                vendorService,
                webHelper,
                workContext,
                workflowMessageService,
                localizationSettings,
                orderSettings,
                paymentSettings,
                rewardPointsSettings,
                shippingSettings,
                taxSettings
            )
        {
            _localizationService = localizationService;
            _pluginService = pluginService;
        }

        public override async Task<PlaceOrderResult> PlaceOrderAsync(ProcessPaymentRequest processPaymentRequest)
        {
            var placeOrderResult = await base.PlaceOrderAsync(processPaymentRequest);

            if (placeOrderResult.Success)
            {
                var pluginDescriptor = await _pluginService.GetPluginDescriptorBySystemNameAsync<IPlugin>(RetargetingDefaults.SystemName);
                if (pluginDescriptor == null)
                    throw new Exception(await _localizationService.GetResourceAsync("Plugins.Widgets.Retargeting.ExceptionLoadPlugin"));

                if (pluginDescriptor.Instance<IPlugin>() is not RetargetingPlugin plugin)
                    throw new Exception(await _localizationService.GetResourceAsync("Plugins.Widgets.Retargeting.ExceptionLoadPlugin"));

                // await plugin.SendOrderAsync(placeOrderResult.PlacedOrder.Id);
            }

            return placeOrderResult;
        }
    }
}
