provider "azurerm" {
  features {}

  subscription_id = "333ed9dc-21a2-4deb-88c9-919eff757cda"
}

resource "azurerm_resource_group" "elysian_bazaar_rg" {
  name     = "ElysianBazaar-Resources"      
  location = "North Europe"                 
}

resource "azurerm_application_insights" "elysian_bazaar_ai" {
  name                = "ElysianBazaar-Insights"            
  location            = azurerm_resource_group.elysian_bazaar_rg.location
  resource_group_name = azurerm_resource_group.elysian_bazaar_rg.name
  application_type    = "web"                               
}

output "app_insights_instrumentation_key" {
  value = azurerm_application_insights.elysian_bazaar_ai.instrumentation_key
  sensitive = true
}

output "app_insights_connection_string" {
  value = azurerm_application_insights.elysian_bazaar_ai.connection_string
  sensitive = true
}
