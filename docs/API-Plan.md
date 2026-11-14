# RaceDay API Plan

| Method | Route | Who | Notes |
|---|---|---|---|
| POST | /api/auth/register | Anyone | Hashes password with BCrypt |
| POST | /api/auth/login | Anyone | Sets session UserId, Role, FullName |
| POST | /api/auth/logout | Anyone | Clears session |
| GET | /api/auth/me | Logged in | Current user |
| GET | /api/events?type= | Anyone | Filter by Run / Walk / Cycle |
| GET | /api/events/{id} | Anyone | |
| POST | /api/events | Organiser | 403 for anyone else |
| PUT | /api/events/{id} | Organiser | |
| DELETE | /api/events/{id} | Organiser | |
| GET/POST | /api/categories | Anyone | 3 categories seeded (5km, 10km, 21km) |
| GET | /api/enrolments | Anyone | |
| GET | /api/enrolments/mine/{userId} | Participant | |
| GET | /api/enrolments/event/{eventId} | Organiser | |
| POST | /api/enrolments | Participant | 409 if already enrolled |
| POST | /api/results | Organiser | 403 for anyone else |
| GET | /api/results/my/{userId} | Participant | Position + TotalFinishers |
| GET/PUT | /api/users | | Profile |
| POST | /api/upload/banner | Anyone | Azure Blob (falls back to wwwroot/banners) |
