//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

#ifndef ESP32_OS_H
#define ESP32_OS_H

#include <string.h>
#include <stdlib.h>
#include <sys/time.h>
#include <time.h>

#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "freertos/timers.h"
#include "freertos/event_groups.h"
#include "esp_system.h"
#include "esp_attr.h"
#include "esp_log.h"
#include "task.h"
#include "nvs_flash.h"

// TODO ONly with network
#include "esp_wifi.h"
#include "esp_wpa2.h"
#include "esp_eth.h"
#include "esp_event_loop.h"

#include "esp_timer.h"

#include "spi_master.h"
#include "gpio.h"
#include "i2c.h"
#include "uart.h"
#include "ledc.h"
#include "adc.h"
#include "dac.h"
#include "timer.h"
#include "esp_spiffs.h"
#include "pcnt.h"

// Uncomment to support Ethernet
//#define ESP32_ETHERNET_SUPPORT

#endif // ESP32_OS_H