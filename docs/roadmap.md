## FamUnion Roadmap

#### Q&A for Reunion Organizers

1. What are/were some pain points around organizing a family reunion?
1. Are/Were there aspects you wish were automated? If so, which ones?
1. What were some highlights?
1. What would you change about the process?
1. Do you feel like you reached everyone you wanted to include?

### MVP
#### Authentication (Auth0)
- [ ] Email/Password
- [ ] Facebook
- [ ] Google
#### Create reunion
- [ ] Reunion Name
- [ ] Description (optional)
- [ ] Start Date
- [ ] City
- [ ] End Date (optional)
- [ ] Add attendee emails (optional)
- [ ] Organizer Contact Details (optional)
#### Attend reunion
- [ ] Manage reunion profile
    - [ ] Phone Number
    - [ ] Picture
    - [ ] Attendance
    - [ ] Lodging Accomodations
        - [ ] Distance to each event (?)
        - [ ] Status
            - [ ] Organizer coordinated
            - [ ] Self coordinated
            - [ ] N/A
    - [ ] Family Management (?)
        - [ ] Family members (spouses/minors)
    - [ ] Requested sizes (T-shirt)
    - [ ] Notifications
        - [ ] Notification Types
            - [ ] App
            - [ ] Text (requires phone number and provider)
        - [ ] Notification Timing
            - [ ] Reunion
                - [ ] X days/weeks prior to Start Date
            - [ ] Event
                - [ ] X days/hours prior to Start Time
                - [ ] Travel time from current location
- [ ] View reunion details
- [ ] View events
    - [ ] View next event (during reunion)
    - [ ] See events on map geolocated _(Premium?)_
- [ ] Export reunion calendar to personal calendar
    - [ ] iCal/iOS
    - [ ] Outlook
    - [ ] Google/Android
#### Manage reunion
- [ ] **Manage announcements**
    - [ ] Add announcement
        - [ ] AnnounceMessage
        - [ ] AnnounceDate
        - [ ] ExpireDate (optional)
        - [ ] Priority
            - [ ] URGENT
            - [ ] INFO
    - [ ] Schedule announcement
        - [ ] ScheduledDate (AnnounceDate in the future)
        - [ ] Trigger notifications at AnnounceDate
    - [ ] Expire announcement (via Expire Date)
- [ ] Send attendee invitations
    - [ ] Need SMTP service/server
    - [ ] Allow sign up through app by passcode (OTP) (?)
- [ ] **Manage attendees**
    - [ ] Add attendee
    - [ ] Update attendance
    - [ ] Block attendee (?)
- [ ] **Manage organizers**
    - [ ] Add organizer
    - [ ] Remove organizer
- [ ] **Add event**
    - [ ] Event name
    - [ ] Description
    - [ ] Dress Attire
        - [ ] Casual
        - [ ] Semi-Formal
        - [ ] Formal
        - [ ] See Description
    - [ ] Date/Time
    - [ ] Address
- [ ] **Add lodging**
    - [ ] Lodging name
    - [ ] Address
    - [ ] Details

#### Next Iteration
- [ ] Organizer MFA
- [ ] Payments https://www.entrepreneur.com/article/286006
    - [ ] PayPal
    - [ ] Apple Pay
    - [ ] Google Pay
    - [ ] Venmo
    - [ ] Zelle
    - [ ] Square
- [ ] Budget
    - [ ] Expenses
    - [ ] Payments from attendees (if applicable)
    - [ ] Balance Sheet
- [ ] Public Attendee (allow download of contact info to other attendees)
    - [ ] Export contact info for:
        - [ ] iOS
        - [ ] Android
        - [ ] Excel (web) (?)
- [ ] Events created by attendees
- [ ] (Premium) Upload Reunion/Event Photos/Videos/Media
    - [ ] Integrate with 3rd party photo storage?
        - [ ] Needs API
        - [ ] Low cost entry OR enterprise license
    - [ ] Provide access to all emails on the attendee list
- [ ] Provide lodging recommendations based on city, location of events
- [ ] Provide event recommendations