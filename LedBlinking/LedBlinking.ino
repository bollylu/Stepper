const int RedLed = 10;
const int GreenLed = 11;
const int BlueLed = 12;
const int YellowLed = 13;

void setup() {
  pinMode(RedLed, OUTPUT);
  pinMode(GreenLed, OUTPUT);
  pinMode(BlueLed, OUTPUT);
  pinMode(YellowLed, OUTPUT);
}

void loop() {

  for (int i = RedLed; i <= YellowLed; i++) {
    digitalWrite(i, HIGH);
    delay(100);
  }

  delay(1000);

  for (int i = RedLed; i <= YellowLed; i++) {
    digitalWrite(i, LOW);
    delay(100);
  }

  delay(1000);

}
