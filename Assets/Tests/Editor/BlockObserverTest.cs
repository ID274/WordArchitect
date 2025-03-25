using NUnit.Framework;
using UnityEngine;

public class BlockObserverTest
{
    GameObject testSubject;
    GameObject testObserver;

    MockBlockObserverSubject subjectObject;
    MockBlockObserver observerObject;
    IBlockObserverSubject observerSubjectInterface;

    [SetUp]
    public void SetUp()
    {
        testSubject = new GameObject();
        testObserver = new GameObject();

        subjectObject = testSubject.AddComponent<MockBlockObserverSubject>();
        observerObject = testObserver.AddComponent<MockBlockObserver>();
        observerSubjectInterface = subjectObject;

        subjectObject.AddObserver(observerObject);
    }

    [Test]
    public void NotifyObserversTest()
    {
        char character = 'A';
        Color color = Color.green;

        observerSubjectInterface.NotifyObservers(character, color);

        Assert.AreEqual(color, observerObject.color);
        Assert.AreEqual(character, observerObject.character);
    }

    [TearDown]
    public void TearDown()
    {
        testSubject = null;
        testObserver = null;
        
        subjectObject = null;
        observerObject = null;

        observerSubjectInterface = null;
    }
}
