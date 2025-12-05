# Shukuma - Fitness Workout Application

> An interactive web-based fitness platform that transforms traditional workout card decks into a digital, tracked, and gamified experience.
]

##  Project Overview

Shukuma is a production-ready fitness application that digitizes a 52-card workout deck, enabling users to:
- Track workouts with real-time progress monitoring
- Compete on global leaderboards
- View detailed workout history and analytics
- Manage personal fitness profiles
- Receive guided video tutorials for each exercise

**Status:** Active Development | Phase 2 Features in Progress

## Key Features

### Current (v1.0)
- User Authentication & Session Management
- Interactive Card-Based Workout System
- Real-Time Progress Tracking (stopwatch, cards completed, progress bar)
- Workout History & Analytics
- Global Leaderboard (3 ranking modes)
- User Profile Management
- Review & Rating System
- Responsive Design (Mobile/Tablet/Desktop)

### In Development (v2.0)
- Achievement Badge System
- Email Notifications
- Social Challenge Features
- Advanced Analytics Dashboard
- Mobile App (iOS/Android)

## Tech Stack

**Backend:**
- ASP.NET Core 6.0 MVC
- C# 10.0
- Firebase/Firestore (NoSQL Database)
- AutoMapper for Object Mapping

**Frontend:**
- Razor Pages
- Bootstrap 5
- JavaScript (ES6+)
- HTML5/CSS3

**DevOps:**
- Docker for Containerization
- Git/GitHub for Version Control
- Render/Azure for Deployment
- Environment Variable Configuration

## Project Structure
```
shukuma/
├── shukuma/                    # Main Web Application
├── shukuma.domain/             # Domain Models & Interfaces
├── shukuma.persistence.firebase/ # Data Access Layer
├── docs/                       # Documentation
└── Dockerfile                  # Container Configuration
```

## Getting Started

### Prerequisites
- .NET 6.0 SDK
- Firebase Account
- Docker (optional)

### Installation

1. Clone the repository
```bash
git clone https://github.com/NzamaE/Shukuma-App.git
cd Shukuma-App
```

2. Configure Firebase
   - Create `appsettings.json` from template
   - Add your Firebase credentials

3. Run the application
```bash
dotnet run --project shukuma
```

Visit `https://localhost:7XXX`

## Documentation

Comprehensive documentation available in `/docs`:
- User Guide
- Technical Documentation
- API Specifications
- Scaling Recommendations
- Testing Guide

## Screenshots

[Add screenshots of Dashboard, Workout Page, Leaderboard]

## Architecture

Built with Clean Architecture principles:
- **Presentation Layer:** ASP.NET Core MVC
- **Application Layer:** Controllers, ViewModels
- **Domain Layer:** Models, Interfaces, Business Logic
- **Infrastructure Layer:** Firebase/Firestore Implementation

## Project Metrics

- **10+** Major Features Implemented
- **2** Database Collections
- **13** Service Methods
- **50+** Test Cases Documented
- **8,000+** Lines of Code
- **5** Comprehensive Documentation Guides

## Roadmap

### Phase 2 (Q1 2025)
- Achievement & Badge System
- Email Notification Service
- Enhanced Security (2FA, Password Reset)

### Phase 3 (Q2 2025)
- Social Features (Friends, Challenges)
- Advanced Analytics Dashboard
- Custom Workout Builder

### Phase 4 (Q3 2025)
- Mobile Apps (React Native)
- Wearable Device Integration
- AI-Powered Recommendations

## Contributing

This is a personal project, but feedback and suggestions are welcome! Feel free to:
- Report bugs via Issues
- Suggest features
- Provide feedback on UX/UI

## License

MIT License - See LICENSE file for details

## Author

**Ernest Nzama**
- GitHub: [@NzamaE](https://github.com/NzamaE)
- LinkedIn: [Ernest Nzama](www.linkedin.com/in/ernestsoftwaredeveloper)
- Email: nzamaernest1@gmail.com

## Acknowledgments

- Original prototype concept provided as starting point
- Enhanced and expanded independently
- Built as demonstration of full-stack development capabilities

---

**If you find this project interesting, consider giving it a star!**

*Last Updated: December 2024*
