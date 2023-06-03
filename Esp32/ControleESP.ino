#define pinX1 26
#define pinY1 2
#define pinB1 25

#define pinX2 4
#define pinY2 15
#define pinB2 18

bool flB2 = false;
bool blB2;

void setup() {
  Serial.begin(115200);
  pinMode(pinX1, INPUT);
  pinMode(pinX2, INPUT);
  pinMode(pinY1, INPUT);
  pinMode(pinY2, INPUT);
  pinMode(pinB1, INPUT_PULLUP);
  pinMode(pinB2, INPUT_PULLUP);
}

void loop() {
  int x1 = analogRead(pinX1);
  int y1 = analogRead(pinY1);
  int b1 = digitalRead(pinB1);

  int x2 = analogRead(pinX2);
  int y2 = analogRead(pinY2);
  int b2 = digitalRead(pinB2);

  int spd = 1;
  int run = 2;

  Serial.print((x1 <= 1800 && x1 > 500) ? -1 * (spd) : (x1 <= 500)               ? -(run) * (spd)
                                                     : (x1 >= 2000 && x1 < 2500) ? 1 * (spd)
                                                     : (x1 >= 2500)              ? (run) * (spd)
                                                                                 : 0);
  Serial.print(",");
  Serial.print((y1 <= 1250 && y1 > 700) ? -1 * (spd) : (y1 <= 700)               ? -(run) * (spd)
                                                     : (y1 >= 1500 && y1 < 3000) ? 1 * (spd)
                                                     : (y1 >= 3000)              ? (run) * (spd)
                                                                                 : 0);
  Serial.print(",");
  Serial.print((b1 == 1) ? 0 : 1);
  Serial.print(",");
  Serial.print((x2 <= 1800 && x2 > 500) ? -1 * (spd) : (x2 <= 500)               ? -(run) * (spd)
                                                     : (x2 >= 2000 && x2 < 2500) ? 1 * (spd)
                                                     : (x2 >= 2500)              ? (run) * (spd)
                                                                                 : 0);
  Serial.print(",");
  Serial.print((y2 <= 1700 && y2 > 900) ? -1 * (spd) : (y2 <= 900)               ? -(run) * (spd)
                                                     : (y2 >= 2000 && y2 < 2500) ? 1 * (spd)
                                                     : (y2 >= 2500)              ? (run) * (spd)
                                                                                 : 0);
  Serial.print(",");
  if (b2 == 0) {
    if (!flB2) {
      blB2 = true;
    }
    flB2 = true;
  }
  if (b2 == 1 && flB2) {
    flB2 = false;
  }
  Serial.println(blB2);
  blB2 = false;

  delay(200);
}