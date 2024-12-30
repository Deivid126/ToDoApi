using FluentAssertions;
using ToDo.Domain.Entities;

namespace ToDo.Teste.Domain.Entities
{
    public class EntityTests
    {
        [Fact]
        public void Entity_Should_Initialize_With_Correct_Default_Values()
        {
            // Act
            var entity = new Entity();
            entity.UpdateCreateDate(null);

            // Assert
            entity.Id.Should().NotBeEmpty();
            entity.CreateDate.Should();
            entity.DeleteDate.Should().BeNull();
        }

        [Fact]
        public void Entity_Should_Have_Unique_Id_For_Each_Instance()
        {
            // Act
            var entity1 = new Entity();
            var entity2 = new Entity();

            // Assert
            entity1.Id.Should().NotBe(entity2.Id);
        }

        [Fact]
        public void CreateDate_And_UpdateDate_Should_Be_Equal_On_Initialization()
        {
            // Act
            var entity = new Entity();

            // Assert
            entity.CreateDate.Should().BeCloseTo(entity.UpdateDate, TimeSpan.FromMilliseconds(10));
        }

        [Fact]
        public void DeleteDate_Should_Be_Assignable()
        {
            // Arrange
            var entity = new Entity();
            var deleteDate = DateTime.Now;

            // Act
            entity.GetType().GetProperty("DeleteDate").SetValue(entity, deleteDate);

            // Assert
            entity.DeleteDate.Should().Be(deleteDate);
        }
    }
}
