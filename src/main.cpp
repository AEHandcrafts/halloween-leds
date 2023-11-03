#include <Arduino.h>
#include <FastLED.h>

#define LED_TYPE APA102
#define COLOR_ORDER BGR
#define NUM_LEDS 300
#define DATA_PIN 1
#define CLOCK_PIN 2

CRGB leds[NUM_LEDS];

void setup() {
    FastLED.addLeds<LED_TYPE, DATA_PIN, CLOCK_PIN, COLOR_ORDER>(leds, NUM_LEDS);
}

void loop() {
    for (int i = 0; i < NUM_LEDS; i++)
        leds[i] = CRGB::Red;
    FastLED.show();
    delay(1000);
}