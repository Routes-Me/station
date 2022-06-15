**Document Routes**

**Model**
1. StatuseModel : We use This Model to Retuen Errore Or Seccess Message From Interface To controle TO help programmer or user for show Errore Message

2. ApplicationUser And ApplicationRole : We use Identity for Register and Login user In  Application

*Role*
2.1. SuperAdmin --
2.2. Admin
2.3. Driver (User for Bus)
2.4. Promoter (For Promotion Application)
2.5. Inspector (For inspect user in trip)
2.6. COMPANY (For Show all information about all Buses for this Company)


3. ApplicationStation : this Model for Bus Station point 

4. ApplicationRote : this Model For Route Bus 

5. ApplicationRouteStationMap : this Model For Relation bettwen Station and Route, this Relation we will use to find Route Trip 

6. ApplicationTrip : when user search fro go from station1 to station2 and strart trip this model will register user trip in  system 

7. ApplicationWallet : this Model Save User Account in System

8. ApplicationWalletCharging: for register every charche account for each user

9. ApplicationWalletTrip : each Trip have value this valus save in Route 

10. ApplicationCompany: this model to register bus company in system 

11. ApplicationBus : this model to register bus in our system

12. ApplicationBusDriverMap : this model for relation bettwen Deiver(by User Role) and Bus

13. ApplicationInspectionBusMap :this model relation bettwen Bus and Inspector (by User Role) return User Payment Trip Value by Bus

14. ApplicationInspectionUserMap:this model realtion bettwen User (Payment Trip Value) and Inspector (by User Role).

**Model View**
We use Model View To Tack Value From User For More Secirity 

**Interface**

In All Repostry We Work in Same Step :
1. Add : to Add Model to DB
2. Edite : Edite Model In DB
3. Delete :Delete Model In DB
4. Get : Get Model from DB
5. List : List Model From DB  in Som Times whe Use Incloud this for get Related Forgin Key 

- In Next Step We Will Explain Other Step in **Interface** :

Account/RoleRep: this firt interfac buld for create **Role** For User in System and SuperAdmin User 

Account/ILoginRep : For Generate token to use in Athuntication

Account/IRegisterRep: Method 
1. Activate : Active Login User
2. Deactivate:in this case User Can't Login to system
3. ActiveCode:user register by phone number in this function check code and confirm phone number 
4. EditeImage : Edite User Image 
5. RestUser : To Rest User Password

Upload/IUploudRep : 
1. AddImageProfile : in this system we save photo in file and save url in database in this function we will upload photo to spcific file 

2. DeleteFileProfile : when delete or upload new photo for new user we will delete old file 

Twillio/ITwillioRep:
1. SendSMS : to send sms to user contain code for active 





